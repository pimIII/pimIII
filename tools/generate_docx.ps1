# Execute este script como Administrador no PowerShell (abrir PowerShell como Admin)
# Ele tenta instalar Chocolatey (se necessário) e depois pandoc, graphviz e OpenJDK
# Em seguida baixa plantuml.jar e gera PNGs a partir dos .puml e por fim gera o DOCX com pandoc

Set-StrictMode -Version Latest

# Verifica se está sendo executado como administrador
function Test-IsAdministrator {
    $currentIdentity = [System.Security.Principal.WindowsIdentity]::GetCurrent()
    $principal = New-Object System.Security.Principal.WindowsPrincipal($currentIdentity)
    return $principal.IsInRole([System.Security.Principal.WindowsBuiltInRole]::Administrator)
}

if (-not (Test-IsAdministrator)) {
    Write-Error "Este script precisa ser executado como Administrador. Abra PowerShell como Administrador e execute novamente."
    exit 1
}

# Instala Chocolatey se estiver ausente
if (-not (Get-Command choco -ErrorAction SilentlyContinue)) {
    Write-Host "Chocolatey não encontrado. Instalando Chocolatey..."
    Set-ExecutionPolicy Bypass -Scope Process -Force
    iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
} else {
    Write-Host "Chocolatey já instalado."
}

# Instala pacotes necessários
choco install -y pandoc graphviz openjdk

# Baixa plantuml.jar para a pasta tools (se não existir)
$plantumlJar = Join-Path $PSScriptRoot 'plantuml.jar'
if (-not (Test-Path $plantumlJar)) {
    Write-Host "Baixando plantuml.jar..."
    Invoke-WebRequest -Uri 'https://www.plantuml.com/plantuml.jar' -OutFile $plantumlJar
} else {
    Write-Host "plantuml.jar já existe em $plantumlJar"
}

# Gerar imagens a partir dos .puml na pasta docs
Write-Host "Gerando imagens PlantUML (PNG) a partir de docs/*.puml..."
$puFiles = Get-ChildItem -Path "$PSScriptRoot\..\docs" -Filter "*.puml" -Recurse -ErrorAction SilentlyContinue
if ($puFiles.Count -eq 0) {
    Write-Host "Nenhum arquivo .puml encontrado em docs/. Pulando geração de imagens."
} else {
    foreach ($f in $puFiles) {
        Write-Host "Gerando PNG para $($f.FullName)"
        java -jar $plantumlJar -tpng $f.FullName
    }
}

# Gerar DOCX com pandoc
Write-Host "Gerando DOCX com pandoc..."
Push-Location $PSScriptRoot\..\
try {
    pandoc docs/documentacao_completa.md docs/analise_requisitos.md docs/modelagem.md docs/casos_de_uso_detalhado.md --toc -s -o docs/documentacao_completa.docx -V geometry:margin=1in
    Write-Host "Arquivo gerado: docs/documentacao_completa.docx"
} catch {
    Write-Error "Falha ao executar pandoc: $_"
} finally {
    Pop-Location
}

Write-Host "Concluído."