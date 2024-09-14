# MyP_Chat - FRIDA HUITRÓN BADILLO

# Proyecto de Chat Basado en Sockets - Cliente

Este es un proyecto de chat basado en sockets desarrollado en C#, en el cual los usuarios pueden conectarse a un servidor central, intercambiar mensajes y recibir notificaciones cuando nuevos usuarios se conectan.

## **Dependencias**

Este proyecto utiliza las siguientes dependencias:

1. **.NET Core SDK**  
   Necesitas instalar .NET Core SDK para compilar y ejecutar el proyecto.  
   [Descargar .NET Core SDK](https://dotnet.microsoft.com/download)

2. **Newtonsoft.Json**  
   Esta biblioteca se utiliza para la serialización y deserialización de mensajes en formato JSON.  
   [Newtonsoft.Json en NuGet](https://www.nuget.org/packages/Newtonsoft.Json/)

## **Instalación de las dependencias**

Para instalar las dependencias, sigue los siguientes pasos:

1. **Instalar .NET Core SDK**:  
   Puedes instalarlo siguiendo las instrucciones del sitio oficial:  
   [Instrucciones para .NET Core SDK](https://dotnet.microsoft.com/download)

2. **Instalar Newtonsoft.Json**:  
   Ejecuta el siguiente comando en el directorio del proyecto para instalar la biblioteca `Newtonsoft.Json`:
   ```bash
   dotnet add package Newtonsoft.Json
## **Cómo usar el cliente en la terminal de Linux**

Para ejecutar el cliente en Linux, sigue los siguientes pasos:

### **1. Clonar el repositorio**

Primero, clona el repositorio del proyecto (suponiendo que está alojado en GitHub o un repositorio similar):

```bash
git clone https://github.com/tu_usuario/tu_proyecto.git
cd tu_proyecto/Cliente
```

### **2. Compilar el proyecto**

Asegúrate de que tienes el SDK de .NET Core instalado en tu sistema. Luego, puedes compilar el proyecto con el siguiente comando:

```bash
dotnet build
```

### **3. Ejecutar el cliente**

Una vez que el proyecto esté compilado, ejecuta el cliente usando el siguiente comando:

```bash
dotnet run
```

El cliente solicitará que ingreses un nickname para conectarte al servidor. Si el nickname ya está en uso, se te pedirá que ingreses otro. Después de la conexión exitosa, podrás comenzar a enviar mensajes.

## **Instrucciones de uso del cliente**

1. **Conexión al servidor**:  
   Al iniciar el cliente, se intentará conectar al servidor en el puerto `12500`. Si la conexión es exitosa, se te pedirá que ingreses un nickname.

2. **Intercambio de mensajes**:  
   Una vez conectado, podrás enviar mensajes a otros usuarios conectados. Los mensajes serán transmitidos a todos los usuarios, y se mostrarán con colores específicos para diferenciar cada usuario.

3. **Notificación de nuevos usuarios**:  
   Cuando un nuevo usuario se conecte, recibirás una notificación en la terminal mostrando el mensaje `"Nuevo usuario conectado: <username>"` en rojo.

4. **Salir del chat**:  
   Para salir del chat, simplemente escribe `exit` y presiona Enter. Esto cerrará la conexión con el servidor y el cliente se detendrá.
