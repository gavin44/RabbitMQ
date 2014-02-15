param([String]$RabbitDllPath = "not specified")

$RabbitDllPath = Resolve-Path $RabbitDllPath 
Write-Host "Rabbit DLL Path: " 
Write-Host $RabbitDllPath -foregroundcolor green

$absoluteRabbitDllPath = Resolve-Path $RabbitDllPath

Write-Host "Absolute Rabbit DLL Path: " 
Write-Host $absoluteRabbitDllPath -foregroundcolor green

[Reflection.Assembly]::LoadFile($absoluteRabbitDllPath)

Write-Host "Setting up RabbitMQ Connection Factory" -foregroundcolor green
$factory = new-object RabbitMQ.Client.ConnectionFactory
$hostNameProp = [RabbitMQ.Client.ConnectionFactory].GetField(“HostName”)
$hostNameProp.SetValue($factory, “localhost”)

$usernameProp = [RabbitMQ.Client.ConnectionFactory].GetField(“UserName”)
$usernameProp.SetValue($factory, “guest”)

$passwordProp = [RabbitMQ.Client.ConnectionFactory].GetField(“Password”)
$passwordProp.SetValue($factory, “guest”)

$createConnectionMethod = [RabbitMQ.Client.ConnectionFactory].GetMethod(“CreateConnection”, [Type]::EmptyTypes)
$connection = $createConnectionMethod.Invoke($factory, “instance,public”, $null, $null, $null)

Write-Host "Setting up RabbitMQ Model" -foregroundcolor green
$model = $connection.CreateModel()


Write-Host "Creating Exchange" -foregroundcolor green
$exchangeType = [RabbitMQ.Client.ExchangeType]::Topic
$model.ExchangeDeclare("Sample.Exchange", $exchangeType, $true)

Write-Host "Creating Server 1 Queue" -foregroundcolor green
$model.QueueDeclare(“Sample.Queue1”, $true, $false, $false, $null)
$model.QueueBind("Sample.Queue1", " Sample.Exchange", "*.high.*")

Write-Host "Creating Server 2 Queue" -foregroundcolor green
$model.QueueDeclare(“Sample.Queue2”, $true, $false, $false, $null)
$model.QueueBind("Sample.Queue2", "Sample.Exchange", "*.*.cupboard")

Write-Host "Creating Server 3 Queue" -foregroundcolor green
$model.QueueDeclare(“Sample.Queue3”, $true, $false, $false, $null)
$model.QueueBind("Sample.Queue3", "Sample.Exchange", "*.medium.*")
$model.QueueBind("Sample.Queue3", "Sample.Exchange", "corporate.#")


Write-Host "Setup complete"