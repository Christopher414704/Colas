# COLAS EN C# CON RABIT

## Requisitos
#### .NET SDK (versión 6.0 o superior).

#### RabbitMQ instalado y en ejecución.

#### Asegúrate de que RabbitMQ esté en ejecución.

------------



## Ejecuta el proyecto:

1. Asegurarse de que RabbitMQ este en ejecucion
2. Ejecuta el proyecto con:
###### dotnet run

3. El programa enviará un mensaje a la cola testQueue y luego lo recibirá.


El programa enviará un mensaje a la cola testQueue y luego lo recibirá.



# Estructura del Proyecto
#### Program.cs: Punto de entrada del programa.

##### RabbitMQService.cs: Implementación del servicio de RabbitMQ.

##### QueueManager.cs: Clase que gestiona el envío y recepción de mensajes.
