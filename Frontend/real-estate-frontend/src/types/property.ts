// Tipos que coinciden con los DTOs del backend .NET
// Según los requisitos de la prueba técnica

export interface Owner {
  id: string;
  idOwner: string;
  name: string;
  address: string;
  photo: string;
  birthday: string;
}

export interface PropertyImage {
  id: string;
  idPropertyImage: string;
  idProperty: string;
  file: string;
  enabled: boolean;
}

export interface PropertyTrace {
  id: string;
  idPropertyTrace: string;
  dateSale: string;
  name: string;
  value: number;
  tax: number;
  idProperty: string;
}

// Detalles completos de una propiedad (para ver detalles individuales)
export interface PropertyDetail {
  id: string;
  idProperty: string;
  name: string;
  address: string;
  price: number;
  codeInternal: string;
  year: number;
  idOwner: string;
  owner?: Owner;
  mainImage?: string;
  images: PropertyImage[];
  traces: PropertyTrace[];
}

// Lista de propiedades con campos específicos según requisitos:
// IdOwner, Name, Address Property, Price Property and just one image
export interface PropertyListItem {
  id: string;
  idProperty: string;
  name: string;
  address: string;
  price: number;
  year: number;
  ownerName: string;
  mainImage: string;
  // Propiedades adicionales para mejorar la UI
  bedrooms?: number;
  bathrooms?: number;
  area?: number;
  images?: PropertyImage[];
}

// Filtros según requisitos: name, address and range price
export interface PropertyFilters {
  name?: string;
  address?: string;
  minPrice?: number;
  maxPrice?: number;
  page?: number;
  pageSize?: number;
}

// API Response types
export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
}

export interface PaginatedResponse<T> {
  data: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}