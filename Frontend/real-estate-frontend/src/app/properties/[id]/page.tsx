'use client';

import { useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { ArrowLeft, MapPin, Calendar, User, DollarSign } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { PropertyImagePlaceholder } from '@/components/ui/property-image-placeholder';
import { useProperty } from '@/hooks/useProperties';
import Image from 'next/image';

export default function PropertyDetailPage() {
  const params = useParams();
  const router = useRouter();
  const propertyId = params.id as string;
  
  const { data: property, isLoading, error } = useProperty(propertyId);
  const [selectedImage, setSelectedImage] = useState<string | null>(null);

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0,
    }).format(price);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('es-CO');
  };

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="animate-pulse">
            <div className="h-8 bg-gray-200 rounded w-48 mb-6" />
            <div className="h-96 bg-gray-200 rounded mb-6" />
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
              <div className="lg:col-span-2 space-y-4">
                <div className="h-6 bg-gray-200 rounded w-3/4" />
                <div className="h-4 bg-gray-200 rounded w-full" />
                <div className="h-4 bg-gray-200 rounded w-2/3" />
              </div>
              <div className="h-64 bg-gray-200 rounded" />
            </div>
          </div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <h1 className="text-2xl font-bold text-gray-900 mb-4">Error</h1>
          <p className="text-gray-600 mb-4">{error.message}</p>
          <Button onClick={() => router.push('/')}>
            Volver al inicio
          </Button>
        </div>
      </div>
    );
  }

  if (!property) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <h1 className="text-2xl font-bold text-gray-900 mb-4">Propiedad no encontrada</h1>
          <Button onClick={() => router.push('/')}>
            Volver al inicio
          </Button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white shadow-sm border-b">
        <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between h-16">
            <Button
              variant="ghost"
              onClick={() => router.push('/')}
              className="flex items-center gap-2"
            >
              <ArrowLeft className="h-4 w-4" />
              Volver a propiedades
            </Button>
            <h1 className="text-lg font-semibold text-gray-900">
              Detalle de Propiedad
            </h1>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Left Column - Property Details */}
          <div className="lg:col-span-2 space-y-6">
            {/* Main Image */}
            <div className="relative aspect-[16/10] rounded-lg overflow-hidden">
              {(selectedImage || property.mainImage) ? (
                <Image
                  src={(selectedImage || property.mainImage) as string}
                  alt={property.name}
                  fill
                  className="object-cover"
                  sizes="(max-width: 1024px) 100vw, 66vw"
                />
              ) : (
                <PropertyImagePlaceholder name={property.name} />
              )}
              <div className="absolute top-4 left-4">
                <Badge variant="secondary" className="bg-white/90 text-gray-900 font-semibold text-lg">
                  {formatPrice(property.price)}
                </Badge>
              </div>
            </div>

            {/* Property Info */}
            <Card>
              <CardHeader>
                <CardTitle>{property.name}</CardTitle>
                <div className="flex items-center text-gray-600">
                  <MapPin className="h-4 w-4 mr-1" />
                  <span>{property.address}</span>
                </div>
              </CardHeader>
              <CardContent>
                <div className="grid grid-cols-2 gap-4 text-sm">
                  <div className="flex items-center gap-2">
                    <Calendar className="h-4 w-4 text-gray-500" />
                    <span>Año: {property.year}</span>
                  </div>
                  <div className="flex items-center gap-2">
                    <span className="font-medium">Código:</span>
                    <span>{property.codeInternal}</span>
                  </div>
                </div>
              </CardContent>
            </Card>

            {/* Images Grid */}
            {property.images && property.images.length > 0 && (
              <Card>
                <CardHeader>
                  <CardTitle>Galería de Imágenes</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="grid grid-cols-2 md:grid-cols-3 gap-4">
                    {property.images.map((image) => (
                      <button
                        key={image.id}
                        type="button"
                        className={`relative aspect-square rounded-lg overflow-hidden cursor-pointer transition-all hover:ring-2 hover:ring-blue-500 hover:scale-105 focus:outline-none focus:ring-2 focus:ring-blue-600 ${
                          selectedImage === image.file ? 'ring-2 ring-blue-600 scale-105' : ''
                        }`}
                        onClick={() => setSelectedImage(image.file)}
                      >
                        <Image
                          src={image.file}
                          alt="Property image"
                          fill
                          className="object-cover"
                          sizes="(max-width: 768px) 50vw, 33vw"
                        />
                      </button>
                    ))}
                  </div>
                </CardContent>
              </Card>
            )}
          </div>

          {/* Right Column - Owner & Traces */}
          <div className="space-y-6">
            {/* Owner Information */}
            {property.owner && (
              <Card>
                <CardHeader>
                  <CardTitle className="flex items-center gap-2">
                    <User className="h-5 w-5" />
                    Propietario
                  </CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="space-y-3">
                    {property.owner.photo && (
                      <div className="relative w-16 h-16 rounded-full overflow-hidden mx-auto">
                        <Image
                          src={property.owner.photo}
                          alt={property.owner.name}
                          fill
                          className="object-cover"
                          sizes="64px"
                        />
                      </div>
                    )}
                    <div className="text-center">
                      <h3 className="font-medium">{property.owner.name}</h3>
                      <p className="text-sm text-gray-600">{property.owner.address}</p>
                      <p className="text-xs text-gray-500">
                        Nacimiento: {formatDate(property.owner.birthday)}
                      </p>
                    </div>
                  </div>
                </CardContent>
              </Card>
            )}

            {/* Property Traces */}
            {property.traces && property.traces.length > 0 && (
              <Card>
                <CardHeader>
                  <CardTitle className="flex items-center gap-2">
                    <DollarSign className="h-5 w-5" />
                    Historial de Transacciones
                  </CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="space-y-3">
                    {property.traces.map((trace) => (
                      <div
                        key={trace.id}
                        className="border-l-2 border-blue-200 pl-4 py-2"
                      >
                        <div className="font-medium text-sm">{trace.name}</div>
                        <div className="text-xs text-gray-500">
                          {formatDate(trace.dateSale)}
                        </div>
                        <div className="text-sm">
                          <span className="font-medium">Valor:</span> {formatPrice(trace.value)}
                        </div>
                        <div className="text-sm">
                          <span className="font-medium">Impuesto:</span> {formatPrice(trace.tax)}
                        </div>
                      </div>
                    ))}
                  </div>
                </CardContent>
              </Card>
            )}

            {/* Contact Actions */}
            <Card>
              <CardHeader>
                <CardTitle>¿Interesado en esta propiedad?</CardTitle>
              </CardHeader>
              <CardContent className="space-y-3">
                <Button className="w-full">
                  Contactar Agente
                </Button>
                <Button variant="outline" className="w-full">
                  Agendar Visita
                </Button>
                <Button variant="ghost" className="w-full">
                  Más Información
                </Button>
              </CardContent>
            </Card>
          </div>
        </div>
      </main>
    </div>
  );
}