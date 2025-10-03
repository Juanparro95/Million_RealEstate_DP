import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import { PropertyListItem, PropertyDetail, PropertyFilters } from '@/types/property';
import { PropertyService } from '@/services/property.service';

interface PropertyState {
  // State
  properties: PropertyListItem[];
  selectedProperty: PropertyDetail | null;
  filters: PropertyFilters;
  loading: boolean;
  error: string | null;
  
  // Pagination
  currentPage: number;
  pageSize: number;
  totalProperties: number;

  // Actions
  setFilters: (filters: Partial<PropertyFilters>) => void;
  clearFilters: () => void;
  setError: (error: string | null) => void;
  setLoading: (loading: boolean) => void;
  
  // Async actions
  fetchProperties: () => Promise<void>;
  fetchPropertyById: (id: string) => Promise<void>;
  searchProperties: (searchFilters: PropertyFilters) => Promise<void>;
  refreshProperties: () => Promise<void>;
  
  // Pagination actions
  setPage: (page: number) => void;
  setPageSize: (pageSize: number) => void;
}

const initialFilters: PropertyFilters = {
  name: '',
  address: '',
  minPrice: undefined,
  maxPrice: undefined,
  page: 1,
  pageSize: 12,
};

export const usePropertyStore = create<PropertyState>()(
  devtools(
    (set, get) => ({
      // Initial state
      properties: [],
      selectedProperty: null,
      filters: initialFilters,
      loading: false,
      error: null,
      currentPage: 1,
      pageSize: 12,
      totalProperties: 0,

      // Synchronous actions
      setFilters: (newFilters) => {
        set((state) => ({
          filters: { ...state.filters, ...newFilters },
        }));
      },

      clearFilters: () => {
        set({ filters: initialFilters, currentPage: 1 });
      },

      setError: (error) => {
        set({ error });
      },

      setLoading: (loading) => {
        set({ loading });
      },

      setPage: (page) => {
        set({ currentPage: page });
        get().fetchProperties();
      },

      setPageSize: (pageSize) => {
        set({ pageSize, currentPage: 1 });
        get().fetchProperties();
      },

      // Async actions
      fetchProperties: async () => {
        const { filters, currentPage, pageSize } = get();
        
        set({ loading: true, error: null });
        
        try {
          const searchFilters = {
            ...filters,
            page: currentPage,
            pageSize,
          };
          
          const properties = await PropertyService.getProperties(searchFilters);
          
          set({
            properties,
            loading: false,
            totalProperties: properties.length,
          });
        } catch (error) {
          set({
            error: error instanceof Error ? error.message : 'Error al cargar propiedades',
            loading: false,
          });
        }
      },

      fetchPropertyById: async (id: string) => {
        set({ loading: true, error: null });
        
        try {
          const property = await PropertyService.getPropertyById(id);
          set({
            selectedProperty: property,
            loading: false,
          });
        } catch (error) {
          set({
            error: error instanceof Error ? error.message : 'Error al cargar la propiedad',
            loading: false,
          });
        }
      },

      searchProperties: async (searchFilters: PropertyFilters) => {
        set({ 
          filters: { ...get().filters, ...searchFilters },
          currentPage: 1,
          loading: true, 
          error: null 
        });
        
        try {
          const properties = await PropertyService.getProperties({
            ...searchFilters,
            page: 1,
            pageSize: get().pageSize,
          });
          
          set({
            properties,
            loading: false,
          });
        } catch (error) {
          set({
            error: error instanceof Error ? error.message : 'Error en la búsqueda',
            loading: false,
          });
        }
      },

      refreshProperties: async () => {
        await get().fetchProperties();
      },
    }),
    {
      name: 'property-store',
    }
  )
);

// Selectors para optimizar re-renders
export const useProperties = () => usePropertyStore((state) => state.properties);
export const useSelectedProperty = () => usePropertyStore((state) => state.selectedProperty);
export const useFilters = () => usePropertyStore((state) => state.filters);
export const useLoading = () => usePropertyStore((state) => state.loading);
export const useError = () => usePropertyStore((state) => state.error);
export const usePagination = () => usePropertyStore((state) => ({
  currentPage: state.currentPage,
  pageSize: state.pageSize,
  totalProperties: state.totalProperties,
}));