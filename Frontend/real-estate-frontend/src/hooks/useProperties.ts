import { useQuery } from '@tanstack/react-query';
import { PropertyService } from '@/services/property.service';
import { PropertyFilters } from '@/types/property';

// Query keys - Simplified for technical test requirements
export const propertyKeys = {
  all: ['properties'] as const,
  lists: () => [...propertyKeys.all, 'list'] as const,
  list: (filters: PropertyFilters) => [...propertyKeys.lists(), filters] as const,
  details: () => [...propertyKeys.all, 'detail'] as const,
  detail: (id: string) => [...propertyKeys.details(), id] as const,
};

// Hook for fetching properties list with filters
// Requirement: "A list of properties, fetched from the API"
// Requirement: "Filters for searching properties (name, address and range price)"
export const useProperties = (filters: PropertyFilters) => {
  return useQuery({
    queryKey: propertyKeys.list(filters),
    queryFn: () => PropertyService.getProperties(filters),
    staleTime: 5 * 60 * 1000, // 5 minutos
    gcTime: 10 * 60 * 1000, // 10 minutos
    retry: 2,
    refetchOnWindowFocus: false,
  });
};

// Hook for fetching individual property details
// Requirement: "Option to view more details about individual properties"
export const useProperty = (id: string) => {
  return useQuery({
    queryKey: propertyKeys.detail(id),
    queryFn: () => PropertyService.getPropertyById(id),
    enabled: !!id,
    staleTime: 5 * 60 * 1000,
    gcTime: 10 * 60 * 1000,
    retry: 2,
  });
};