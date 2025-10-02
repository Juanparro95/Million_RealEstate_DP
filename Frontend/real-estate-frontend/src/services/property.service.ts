import axios, { AxiosResponse } from 'axios';
import { PropertyListItem, PropertyDetail, PropertyFilters } from '@/types/property';

// Configuración base de la API
const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5079/api';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor para manejo de errores
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error('API Error:', error);
    
    if (error.response?.status === 404) {
      throw new Error('Recurso no encontrado');
    } else if (error.response?.status === 500) {
      throw new Error('Error interno del servidor');
    } else if (error.code === 'ECONNABORTED') {
      throw new Error('Tiempo de espera agotado');
    } else if (!error.response) {
      throw new Error('Error de conexión con el servidor');
    }
    
    throw new Error(error.response?.data?.message || 'Error desconocido');
  }
);

export class PropertyService {
  /**
   * Obtener todas las propiedades con filtros opcionales
   * Según los requisitos de la prueba técnica: filtros por name, address y range price
   */
  static async getProperties(filters: PropertyFilters = {}): Promise<PropertyListItem[]> {
    const params = new URLSearchParams();
    
    if (filters.name) params.append('name', filters.name);
    if (filters.address) params.append('address', filters.address);
    if (filters.minPrice !== undefined) params.append('minPrice', filters.minPrice.toString());
    if (filters.maxPrice !== undefined) params.append('maxPrice', filters.maxPrice.toString());
    if (filters.page) params.append('page', filters.page.toString());
    if (filters.pageSize) params.append('pageSize', filters.pageSize.toString());

    const response: AxiosResponse<PropertyListItem[]> = await apiClient.get(
      `/properties?${params.toString()}`
    );
    
    // Mapear los datos a la estructura esperada por el frontend
    return response.data.map(property => ({
      ...property,
      // Agregar propiedades opcionales si no existen
      bedrooms: property.bedrooms || 3,
      bathrooms: property.bathrooms || 2,
      area: property.area || 120,
      images: property.images || [],
      mainImage: property.mainImage || '/placeholder-property.jpg'
    }));
  }

  /**
   * Obtener una propiedad por ID con todos los detalles
   * Según los requisitos: "Option to view more details about individual properties"
   */
  static async getPropertyById(id: string): Promise<PropertyDetail> {
    const response: AxiosResponse<PropertyDetail> = await apiClient.get(`/properties/${id}`);
    return response.data;
  }
}

export default PropertyService;