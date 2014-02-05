
param([String]$RabbitDllPath = "not specified")


Write-Host "Rabbit DLL Path: "
Write-Host $RabbitDllPath -foregroundcolor green

$absoluteRabbitDllPath = Resolve-Path $RabbitDllPath

Write-Host "Absolute Rabbit DLL Path: "
Write-Host $absoluteRabbitDllPath -foregroundcolor green

[Reflection.Assembly]::LoadFile($absoluteRabbitDllPath)

Write-Host "Setting up RabbitMQ Connection Factory"
$factory = new-object RabbitMQ.Client.ConnectionFactory
$hostNameProp = [RabbitMQ.Client.ConnectionFactory].GetField("HostName")
$hostNameProp.SetValue($factory, "localhost")

Write-Host "Setting up user"
$usernameProp = [RabbitMQ.Client.ConnectionFactory].GetField("UserName")
$usernameProp.SetValue($factory, "guest")

$passwordProp = [RabbitMQ.Client.ConnectionFactory].GetField("Password")
$passwordProp.SetValue($factory, "guest")

$createConnectionMethod = [RabbitMQ.Client.ConnectionFactory].GetMethod("CreateConnection", [Type]::EmptyTypes)
$connection = $createConnectionMethod.Invoke($factory, "instance,public", $null, $null, $null)

Write-Host "Setting up RabbitMQ Model"
$model = $connection.CreateModel()

Write-Host "Creating Exchange"
$exchangeType = [RabbitMQ.Client.ExchangeType]::Fanout
$model.ExchangeDeclare("Sample.Exchange", $exchangeType, $true)

Write-Host "Creating Server 1 Queue"
$model.QueueDeclare(“Server1.Sample.Queue”, $true, $false, $false, $null)
$model.QueueBind("Server1.Sample.Queue", "Sample.Exchange", "")

Write-Host "Creating Server 2 Queue"
$model.QueueDeclare(“Server2.Sample.Queue”, $true, $false, $false, $null)
$model.QueueBind("Server2.Sample.Queue", "Sample.Exchange", "")

Write-Host "Setup complete"