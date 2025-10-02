import { PropertyListItem } from "@/types/property";
import { PropertyCard, PropertyCardSkeleton } from "./property-card";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { AlertCircle, Search } from "lucide-react";

interface PropertyGridProps {
  readonly properties: PropertyListItem[];
  readonly isLoading: boolean;
  readonly error: string | null;
  readonly onViewDetails: (id: string) => void;
  readonly onPrefetch?: (id: string) => void;
}

export function PropertyGrid({
  properties,
  isLoading,
  error,
  onViewDetails,
  onPrefetch,
}: PropertyGridProps) {
  // Mostrar skeleton mientras carga
  if (isLoading) {
    const skeletonItems = Array.from({ length: 8 }, (_, i) => `skeleton-${Date.now()}-${i}`);
    
    return (
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {skeletonItems.map((key) => (
          <PropertyCardSkeleton key={key} />
        ))}
      </div>
    );
  }

  // Mostrar error si existe
  if (error) {
    return (
      <Alert variant="destructive">
        <AlertCircle className="h-4 w-4" />
        <AlertDescription>{error}</AlertDescription>
      </Alert>
    );
  }

  // Mostrar mensaje cuando no hay propiedades
  if (properties.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-12 text-center">
        <Search className="h-16 w-16 text-gray-300 mb-4" />
        <h3 className="text-lg font-semibold text-gray-900 mb-2">
          No se encontraron propiedades
        </h3>
        <p className="text-gray-500 max-w-md">
          No hay propiedades que coincidan con los criterios de búsqueda actuales.
          Intenta ajustar los filtros o realiza una nueva búsqueda.
        </p>
      </div>
    );
  }

  // Mostrar grid de propiedades
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
      {properties.map((property) => (
        <PropertyCard
          key={property.id}
          property={property}
          onViewDetails={onViewDetails}
          onPrefetch={onPrefetch}
        />
      ))}
    </div>
  );
}

// Componente para mostrar estadísticas rápidas
interface PropertyStatsProps {
  readonly totalProperties: number;
  readonly isLoading: boolean;
}

export function PropertyStats({ totalProperties, isLoading }: PropertyStatsProps) {
  if (isLoading) {
    return (
      <div className="bg-white p-4 rounded-lg shadow-sm border">
        <div className="h-4 bg-gray-200 rounded w-32 animate-pulse" />
      </div>
    );
  }

  const getPropertiesText = () => {
    if (totalProperties === 0) return 'No se encontraron propiedades';
    
    const propertyWord = totalProperties === 1 ? 'propiedad encontrada' : 'propiedades encontradas';
    return `${totalProperties.toLocaleString()} ${propertyWord}`;
  };

  return (
    <div className="bg-white p-4 rounded-lg shadow-sm border">
      <p className="text-sm text-gray-600">
        {getPropertiesText()}
      </p>
    </div>
  );
}