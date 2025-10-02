import Image from "next/image";
import { PropertyListItem } from "@/types/property";
import { Card, CardContent, CardFooter, CardHeader } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { PropertyImagePlaceholder } from "@/components/ui/property-image-placeholder";
import { MapPin, Eye, Bed, Bath, Square } from "lucide-react";

interface PropertyCardProps {
  readonly property: PropertyListItem;
  readonly onViewDetails: (id: string) => void;
  readonly onPrefetch?: (id: string) => void;
}

export function PropertyCard({ 
  property, 
  onViewDetails, 
  onPrefetch 
}: PropertyCardProps) {
  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0,
    }).format(price);
  };

  const handleMouseEnter = () => {
    // Precargar datos de la propiedad al hacer hover
    if (onPrefetch) {
      onPrefetch(property.id);
    }
  };

  return (
    <Card 
      className="group overflow-hidden hover:shadow-lg transition-all duration-300 cursor-pointer"
      onMouseEnter={handleMouseEnter}
    >
      <CardHeader className="p-0">
        <div className="relative aspect-[4/3] overflow-hidden">
          {(() => {
            if (property.mainImage && property.mainImage !== '/placeholder-property.jpg') {
              return (
                <Image
                  src={property.mainImage}
                  alt={property.name}
                  fill
                  className="object-cover group-hover:scale-105 transition-transform duration-300"
                  sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
                />
              );
            }
            
            if (property.images && property.images.length > 0) {
              return (
                <Image
                  src={property.images[0].file}
                  alt={property.name}
                  fill
                  className="object-cover group-hover:scale-105 transition-transform duration-300"
                  sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
                />
              );
            }
            
            return <PropertyImagePlaceholder name={property.name} />;
          })()}
          
          {/* Overlay con precio */}
          <div className="absolute top-4 left-4">
            <Badge variant="secondary" className="bg-white/90 text-gray-900 font-semibold">
              {formatPrice(property.price)}
            </Badge>
          </div>

          {/* Badge de estado si está disponible */}
          <div className="absolute top-4 right-4">
            <Badge variant="default" className="bg-green-500 text-white">
              Disponible
            </Badge>
          </div>
        </div>
      </CardHeader>

      <CardContent className="p-4">
        <div className="space-y-2">
          <h3 className="font-semibold text-lg line-clamp-1 group-hover:text-blue-600 transition-colors">
            {property.name}
          </h3>
          
          <div className="flex items-center text-gray-600 text-sm">
            <MapPin className="h-4 w-4 mr-1 flex-shrink-0" />
            <span className="line-clamp-1">{property.address}</span>
          </div>

          {/* Características de la propiedad */}
          {(property.bedrooms || property.bathrooms || property.area) && (
            <div className="flex items-center gap-4 text-sm text-gray-600 pt-2">
              {property.bedrooms && (
                <div className="flex items-center gap-1">
                  <Bed className="h-4 w-4" />
                  <span>{property.bedrooms}</span>
                </div>
              )}
              {property.bathrooms && (
                <div className="flex items-center gap-1">
                  <Bath className="h-4 w-4" />
                  <span>{property.bathrooms}</span>
                </div>
              )}
              {property.area && (
                <div className="flex items-center gap-1">
                  <Square className="h-4 w-4" />
                  <span>{property.area} m²</span>
                </div>
              )}
            </div>
          )}

          {/* Precio por m² */}
          {property.area && (
            <div className="text-xs text-gray-500">
              {formatPrice(property.price / property.area)} por m²
            </div>
          )}
        </div>
      </CardContent>

      <CardFooter className="p-4 pt-0">
        <Button
          onClick={() => onViewDetails(property.id)}
          className="w-full group-hover:bg-blue-600 transition-colors"
          size="sm"
        >
          <Eye className="h-4 w-4 mr-2" />
          Ver Detalles
        </Button>
      </CardFooter>
    </Card>
  );
}

// Componente para mostrar skeleton loading
export function PropertyCardSkeleton() {
  return (
    <Card className="overflow-hidden">
      <CardHeader className="p-0">
        <div className="aspect-[4/3] bg-gray-200 animate-pulse" />
      </CardHeader>
      <CardContent className="p-4">
        <div className="space-y-2">
          <div className="h-6 bg-gray-200 rounded animate-pulse" />
          <div className="h-4 bg-gray-200 rounded w-3/4 animate-pulse" />
          <div className="flex gap-4 pt-2">
            <div className="h-4 bg-gray-200 rounded w-12 animate-pulse" />
            <div className="h-4 bg-gray-200 rounded w-12 animate-pulse" />
            <div className="h-4 bg-gray-200 rounded w-16 animate-pulse" />
          </div>
        </div>
      </CardContent>
      <CardFooter className="p-4 pt-0">
        <div className="h-9 bg-gray-200 rounded w-full animate-pulse" />
      </CardFooter>
    </Card>
  );
}