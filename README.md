# ApiProductoDocker

Sistema de gestión de productos desarrollado con **.NET 10, Entity Framework Core, PostgreSQL y Vue 3**, preparado para ejecutarse tanto en desarrollo local como mediante Docker.

## Tecnologías

### Backend

* .NET 10
* ASP.NET Core Minimal API
* Entity Framework Core
* PostgreSQL
* Npgsql

### Frontend

* Vue 3
* TypeScript
* Vite

### Infraestructura

* Docker
* Docker Compose
* Nginx

---

## Estructura del proyecto

```text
ApiProductoDocker/
│
├── Api/
│   ├── Dockerfile
│   ├── Api.csproj
│   ├── Program.cs
│   └── ...
│
├── Frontend/
│   ├── Dockerfile
│   ├── package.json
│   ├── src/
│   └── ...
│
├── docker-compose.yml
├── .gitignore
└── README.md
```

---

## Arquitectura

El sistema utiliza la siguiente estructura:

```text
                 ┌──────────────┐
                 │   Vue 3      │
                 │  Frontend    │
                 └──────┬───────┘
                        │ HTTP
                        ▼
                 ┌──────────────┐
                 │ ASP.NET Core │
                 │     API      │
                 └──────┬───────┘
                        │ EF Core
                        ▼
                 ┌──────────────┐
                 │ PostgreSQL   │
                 └──────────────┘
```

El frontend **no se conecta directamente a PostgreSQL**. Todas las operaciones pasan por la API.

---

# Desarrollo local

Para trabajar en desarrollo se puede ejecutar Vue y .NET directamente, mientras PostgreSQL se mantiene dentro de Docker.

### PostgreSQL

Levantar solamente la base de datos:

```bash
docker compose up -d postgres
```

PostgreSQL estará disponible desde el equipo mediante:

```text
localhost:5433
```

### API

Desde la carpeta `Api`:

```bash
dotnet run
```

La API estará disponible en:

```text
http://localhost:5003
```

La cadena de conexión para desarrollo debe utilizar:

```text
Host=localhost
Port=5433
```

### Frontend

Desde la carpeta `Frontend`:

```bash
npm install
npm run dev
```

Vite normalmente estará disponible en:

```text
http://localhost:5173
```

El frontend utiliza una variable de entorno para determinar la URL de la API.

Ejemplo:

```env
VITE_API_URL=http://localhost:5003
```

---

# Ejecutar todo con Docker

Docker permite ejecutar el frontend, backend y PostgreSQL como servicios independientes.

Construir las imágenes y levantar los servicios:

```bash
docker compose up -d --build
```

Ver los contenedores:

```bash
docker compose ps
```

Ver los logs de la API:

```bash
docker compose logs -f api
```

Ver los logs de PostgreSQL:

```bash
docker compose logs -f postgres
```

Para detener los servicios:

```bash
docker compose down
```

> `docker compose down` detiene y elimina los contenedores, pero conserva el volumen de PostgreSQL.

Para eliminar también los datos de PostgreSQL:

```bash
docker compose down -v
```

**Esto elimina permanentemente el volumen de la base de datos.**

---

# Variables de entorno

El proyecto utiliza variables de entorno para configurar PostgreSQL.

Crear un archivo `.env` en la raíz:

```env
POSTGRES_DB=productos
POSTGRES_USER=postgres
POSTGRES_PASSWORD=123456
```

El archivo `.env` no debe subirse al repositorio si contiene credenciales reales.

---

# PostgreSQL y Docker

Existe una diferencia importante entre conectarse desde el equipo y desde un contenedor.

### API ejecutándose localmente

```text
Host=localhost
Port=5433
```

### API ejecutándose dentro de Docker

```text
Host=postgres
Port=5432
```

`postgres` es el nombre del servicio definido en `docker-compose.yml`.

El puerto `5433` solamente es el puerto expuesto hacia el equipo:

```text
localhost:5433
       ↓
PostgreSQL:5432
```

---

# Migraciones

Las migraciones son administradas mediante Entity Framework Core.

Crear una migración:

```bash
dotnet ef migrations add InitialCreate
```

Aplicar las migraciones:

```bash
dotnet ef database update
```

El proyecto también puede aplicar automáticamente las migraciones pendientes al iniciar la API mediante:

```csharp
db.Database.Migrate();
```

Esto permite que la base de datos se actualice automáticamente cuando se levanta el contenedor de la API.

---

# API

La API expone operaciones CRUD para productos.

### Obtener productos

```http
GET /api/products
```

### Obtener un producto

```http
GET /api/products/{id}
```

### Crear producto

```http
POST /api/products
```

Ejemplo:

```json
{
  "name": "Laptop",
  "price": 25000
}
```

### Actualizar producto

```http
PUT /api/products/{id}
```

### Eliminar producto

```http
DELETE /api/products/{id}
```

---

# Comandos útiles de Docker

Levantar:

```bash
docker compose up -d
```

Levantar y reconstruir:

```bash
docker compose up -d --build
```

Detener:

```bash
docker compose stop
```

Iniciar nuevamente:

```bash
docker compose start
```

Reiniciar:

```bash
docker compose restart
```

Detener y eliminar contenedores:

```bash
docker compose down
```

Eliminar contenedores y volumen de PostgreSQL:

```bash
docker compose down -v
```

---

# Requisitos

Para ejecutar el proyecto en desarrollo se necesita:

* .NET 1
