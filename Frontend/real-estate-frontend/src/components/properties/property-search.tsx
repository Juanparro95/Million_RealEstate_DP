import { useState, useEffect } from "react";
import { Search, SlidersHorizontal, X } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet";
import { PropertyFilters } from "@/types/property";

interface PropertySearchProps {
  readonly filters: PropertyFilters;
  readonly onFiltersChange: (filters: Partial<PropertyFilters>) => void;
  readonly onClearFilters: () => void;
  readonly isLoading?: boolean;
}

export function PropertySearch({
  filters,
  onFiltersChange,
  onClearFilters,
  isLoading = false,
}: PropertySearchProps) {
  // Estado local para los inputs (para debounce)
  const [localFilters, setLocalFilters] = useState({
    name: filters.name || '',
    address: filters.address || '',
    minPrice: filters.minPrice || '',
    maxPrice: filters.maxPrice || '',
  });

  // Debounce effect - espera 500ms después de que el usuario deje de escribir
  useEffect(() => {
    const timer = setTimeout(() => {
      onFiltersChange({
        name: localFilters.name || undefined,
        address: localFilters.address || undefined,
        minPrice: localFilters.minPrice ? Number(localFilters.minPrice) : undefined,
        maxPrice: localFilters.maxPrice ? Number(localFilters.maxPrice) : undefined,
      });
    }, 500);

    return () => clearTimeout(timer);
  }, [localFilters.name, localFilters.address, localFilters.minPrice, localFilters.maxPrice]);

  const handleLocalInputChange = (field: keyof typeof localFilters) => (
    e: React.ChangeEvent<HTMLInputElement>
  ) => {
    setLocalFilters(prev => ({
      ...prev,
      [field]: e.target.value,
    }));
  };

  const handleClearFiltersLocal = () => {
    setLocalFilters({
      name: '',
      address: '',
      minPrice: '',
      maxPrice: '',
    });
    onClearFilters();
  };

  const hasActiveFilters = Boolean(
    localFilters.name ||
    localFilters.address ||
    localFilters.minPrice ||
    localFilters.maxPrice
  );

  const formatCurrency = (value: string | number) => {
    if (!value) return '';
    const num = typeof value === 'string' ? Number(value) : value;
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0,
    }).format(num);
  };

  return (
    <div className="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
      {/* Barra principal de búsqueda */}
      <div className="flex flex-col lg:flex-row gap-0 divide-y lg:divide-y-0 lg:divide-x divide-gray-200">
        {/* Búsqueda por nombre */}
        <div className="flex-1 p-4 lg:p-6">
          <div className="flex items-center gap-3">
            <Search className="h-5 w-5 text-gray-400 flex-shrink-0" />
            <div className="flex-1">
              <Label htmlFor="search-name" className="text-xs font-medium text-gray-500 uppercase tracking-wide mb-1 block">
                Nombre de Propiedad
              </Label>
              <Input
                id="search-name"
                placeholder="Ej: Casa, Apartamento, Loft..."
                value={localFilters.name}
                onChange={handleLocalInputChange('name')}
                className="border-0 p-0 h-8 focus-visible:ring-0 focus-visible:ring-offset-0 text-gray-900 placeholder:text-gray-400"
                disabled={isLoading}
              />
            </div>
          </div>
        </div>

        {/* Búsqueda por dirección */}
        <div className="flex-1 p-4 lg:p-6">
          <div className="flex items-center gap-3">
            <Search className="h-5 w-5 text-gray-400 flex-shrink-0" />
            <div className="flex-1">
              <Label htmlFor="search-address" className="text-xs font-medium text-gray-500 uppercase tracking-wide mb-1 block">
                Ubicación
              </Label>
              <Input
                id="search-address"
                placeholder="Ciudad, zona, dirección..."
                value={localFilters.address}
                onChange={handleLocalInputChange('address')}
                className="border-0 p-0 h-8 focus-visible:ring-0 focus-visible:ring-offset-0 text-gray-900 placeholder:text-gray-400"
                disabled={isLoading}
              />
            </div>
          </div>
        </div>

        {/* Botones de acción */}
        <div className="p-4 lg:p-6 flex items-center gap-2 lg:w-auto">
          <Sheet>
            <SheetTrigger asChild>
              <Button variant="outline" className="flex-1 lg:flex-initial relative">
                <SlidersHorizontal className="h-4 w-4 mr-2" />
                Precio
                {(localFilters.minPrice || localFilters.maxPrice) && (
                  <span className="absolute -top-1 -right-1 bg-blue-600 text-white text-xs rounded-full h-4 w-4 flex items-center justify-center">
                    1
                  </span>
                )}
              </Button>
            </SheetTrigger>
            <SheetContent className="w-full sm:max-w-md">
              <SheetHeader>
                <SheetTitle className="text-2xl font-bold">Filtros de Precio</SheetTitle>
                <SheetDescription>
                  Ajusta el rango de precio para encontrar tu propiedad ideal
                </SheetDescription>
              </SheetHeader>
              
              <div className="space-y-8 mt-5 px-5">
                {/* Rango de precio mejorado */}
                <div className="space-y-6">
                  <div className="flex items-center gap-2 text-gray-700">
                    <div>
                      <h3 className="font-semibold text-lg">Rango de Precio</h3>
                      <p className="text-sm text-gray-500">Define tu presupuesto</p>
                    </div>
                  </div>

                  <div className="space-y-4">
                    <div className="space-y-2">
                      <Label htmlFor="minPrice" className="text-sm font-medium text-gray-700">
                        Precio Mínimo
                      </Label>
                      <div className="relative">
                        <span className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-500 text-sm">$</span>
                        <Input
                          id="minPrice"
                          type="number"
                          placeholder="0"
                          value={localFilters.minPrice}
                          onChange={handleLocalInputChange('minPrice')}
                          className="pl-8 h-12"
                          disabled={isLoading}
                        />
                      </div>
                      {localFilters.minPrice && (
                        <p className="text-xs text-gray-500 mt-1">
                          {formatCurrency(localFilters.minPrice)}
                        </p>
                      )}
                    </div>

                    <div className="flex items-center justify-center">
                      <div className="h-px w-full bg-gray-200" />
                      <span className="px-3 text-gray-400 text-sm">a</span>
                      <div className="h-px w-full bg-gray-200" />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="maxPrice" className="text-sm font-medium text-gray-700">
                        Precio Máximo
                      </Label>
                      <div className="relative">
                        <span className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-500 text-sm">$</span>
                        <Input
                          id="maxPrice"
                          type="number"
                          placeholder="Sin límite"
                          value={localFilters.maxPrice}
                          onChange={handleLocalInputChange('maxPrice')}
                          className="pl-8 h-12"
                          disabled={isLoading}
                        />
                      </div>
                      {localFilters.maxPrice && (
                        <p className="text-xs text-gray-500 mt-1">
                          {formatCurrency(localFilters.maxPrice)}
                        </p>
                      )}
                    </div>
                  </div>

                  {/* Rangos predefinidos */}
                  <div className="space-y-2">
                    <Label className="text-sm font-medium text-gray-700">Rangos populares</Label>
                    <div className="grid grid-cols-2 gap-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => {
                          setLocalFilters(prev => ({ ...prev, minPrice: '', maxPrice: '500000000' }));
                        }}
                        className="text-xs"
                      >
                        Hasta $500M
                      </Button>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => {
                          setLocalFilters(prev => ({ ...prev, minPrice: '500000000', maxPrice: '1000000000' }));
                        }}
                        className="text-xs"
                      >
                        $500M - $1.000M
                      </Button>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => {
                          setLocalFilters(prev => ({ ...prev, minPrice: '1000000000', maxPrice: '' }));
                        }}
                        className="text-xs"
                      >
                        Más de $1.000M
                      </Button>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => {
                          setLocalFilters(prev => ({ ...prev, minPrice: '', maxPrice: '' }));
                        }}
                        className="text-xs"
                      >
                        Todos los precios
                      </Button>
                    </div>
                  </div>
                </div>

                {/* Resumen de filtros activos */}
                {(localFilters.minPrice || localFilters.maxPrice) && (
                  <div className="p-4 bg-blue-50 rounded-lg border border-blue-200">
                    <p className="text-sm font-medium text-blue-900 mb-1">
                      Rango seleccionado:
                    </p>
                    <p className="text-lg font-bold text-blue-700">
                      {localFilters.minPrice ? formatCurrency(localFilters.minPrice) : 'Sin mínimo'}
                      {' - '}
                      {localFilters.maxPrice ? formatCurrency(localFilters.maxPrice) : 'Sin máximo'}
                    </p>
                  </div>
                )}
              </div>
            </SheetContent>
          </Sheet>

          {hasActiveFilters && (
            <Button
              variant="ghost"
              size="icon"
              onClick={handleClearFiltersLocal}
              disabled={isLoading}
              className="text-gray-500 hover:text-red-600 hover:bg-red-50"
              title="Limpiar todos los filtros"
            >
              <X className="h-4 w-4" />
            </Button>
          )}
        </div>
      </div>

      {/* Indicador de filtros activos */}
      {hasActiveFilters && (
        <div className="px-4 lg:px-6 py-3 bg-blue-50 border-t border-blue-100">
          <div className="flex items-center justify-between flex-wrap gap-2">
            <div className="flex items-center gap-2 flex-wrap">
              <span className="text-sm font-medium text-blue-900">Filtros activos:</span>
              {localFilters.name && (
                <span className="inline-flex items-center gap-1 px-3 py-1 bg-white rounded-full text-xs font-medium text-gray-700 border border-gray-200">
                  Nombre: {localFilters.name}
                  <button
                    type="button"
                    onClick={() => setLocalFilters(prev => ({ ...prev, name: '' }))}
                    className="hover:text-red-600"
                  >
                    <X className="h-3 w-3" />
                  </button>
                </span>
              )}
              {localFilters.address && (
                <span className="inline-flex items-center gap-1 px-3 py-1 bg-white rounded-full text-xs font-medium text-gray-700 border border-gray-200">
                  Ubicación: {localFilters.address}
                  <button
                    type="button"
                    onClick={() => setLocalFilters(prev => ({ ...prev, address: '' }))}
                    className="hover:text-red-600"
                  >
                    <X className="h-3 w-3" />
                  </button>
                </span>
              )}
              {(localFilters.minPrice || localFilters.maxPrice) && (
                <span className="inline-flex items-center gap-1 px-3 py-1 bg-white rounded-full text-xs font-medium text-gray-700 border border-gray-200">
                  Precio: {localFilters.minPrice ? formatCurrency(localFilters.minPrice) : 'Min'} - {localFilters.maxPrice ? formatCurrency(localFilters.maxPrice) : 'Max'}
                  <button
                    type="button"
                    onClick={() => setLocalFilters(prev => ({ ...prev, minPrice: '', maxPrice: '' }))}
                    className="hover:text-red-600"
                  >
                    <X className="h-3 w-3" />
                  </button>
                </span>
              )}
            </div>
            <Button
              variant="link"
              size="sm"
              onClick={handleClearFiltersLocal}
              className="text-blue-600 hover:text-blue-700 text-xs font-medium"
            >
              Limpiar todo
            </Button>
          </div>
        </div>
      )}
    </div>
  );
}