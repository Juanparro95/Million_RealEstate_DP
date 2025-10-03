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

// Detalles completos de una propiedad
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

export interface PropertyListItem {
  id: string;
  idProperty: string;
  name: string;
  address: string;
  price: number;
  year: number;
  ownerName: string;
  mainImage: string;
  bedrooms?: number;
  bathrooms?: number;
  area?: number;
  images?: PropertyImage[];
}

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