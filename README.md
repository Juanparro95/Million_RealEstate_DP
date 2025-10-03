# Real Estate Platform - Million Luxury

Prueba técnica para Million Luxury. Backend en .NET 9 + MongoDB, Frontend en Next.js 15.

## Qué necesitas tener instalado

- Node.js 18+
- .NET 9 SDK
- MongoDB

## Cómo correr esto

### Backend
```bash
# Asegúrate de tener MongoDB instalado y corriendo.
cd Backend/RealEstateAPI
dotnet restore
dotnet run
```
Abre http://localhost:5079/swagger para ver la API

### Frontend
```bash
cd Frontend/real-estate-frontend
npm install
npm run dev
```
Abre http://localhost:3000

### Tests

**Backend:**
```bash
cd Backend/RealEstateAPI.Tests
dotnet test
```

**Frontend:**
```bash
cd Frontend/real-estate-frontend
npm test
```

## Lo que hace

- Lista de propiedades con filtros (nombre, dirección, precio)
- Detalle de propiedad con galería de imágenes
- API REST con Clean Architecture + CQRS
- Base de datos con datos de prueba que se cargan solos

## Stack

**Backend:** .NET 9, MongoDB, MediatR, AutoMapper, NUnit  
**Frontend:** Next.js 15, React 19, TypeScript, TanStack Query, Tailwind CSS, Jest

## Realizado por:

Juan David Parroquiano Vargas - Prueba técnica Million Luxury  