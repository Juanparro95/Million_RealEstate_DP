'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { PropertySearch } from '@/components/properties/property-search';
import { PropertyGrid, PropertyStats } from '@/components/properties/property-grid';
import { useProperties } from '@/hooks/useProperties';
import { PropertyFilters } from '@/types/property';

const initialFilters: PropertyFilters = {
  name: '',
  address: '',
  minPrice: undefined,
  maxPrice: undefined,
  page: 1,
  pageSize: 12,
};

export default function Home() {
  const router = useRouter();
  const [filters, setFilters] = useState<PropertyFilters>(initialFilters);
  const { data: properties = [], isLoading, error } = useProperties(filters);

  const handleFiltersChange = (newFilters: Partial<PropertyFilters>) => {
    setFilters((prev) => ({
      ...prev,
      ...newFilters,
      page: 1, // Reset to first page when filters change
    }));
  };

  const handleClearFilters = () => {
    setFilters(initialFilters);
  };

  const handleViewDetails = (id: string) => {
    router.push(`/properties/${id}`);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white shadow-sm border-b">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between h-16">
            <div className="flex items-center">
              <h1 className="text-2xl font-bold text-gray-900">
                Million Luxury
              </h1>
              <span className="ml-2 text-sm text-gray-500">Real Estate</span>
            </div>
            <nav className="hidden md:flex space-x-8">
              <button type="button" className="text-gray-700 hover:text-blue-600 px-3 py-2 text-sm font-medium">
                Propiedades
              </button>
              <button type="button" className="text-gray-700 hover:text-blue-600 px-3 py-2 text-sm font-medium">
                Nosotros
              </button>
              <button type="button" className="text-gray-700 hover:text-blue-600 px-3 py-2 text-sm font-medium">
                Contacto
              </button>
            </nav>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Hero Section */}
        <div className="text-center mb-12">
          <h2 className="text-4xl font-bold text-gray-900 mb-4">
            Encuentra tu hogar perfecto
          </h2>
          <p className="text-xl text-gray-600 max-w-2xl mx-auto">
            Descubre propiedades exclusivas en las mejores ubicaciones.
            Calidad, lujo y comodidad en cada detalle.
          </p>
        </div>

        {/* Search Section */}
        <div className="mb-8">
          <PropertySearch
            filters={filters}
            onFiltersChange={handleFiltersChange}
            onClearFilters={handleClearFilters}
            isLoading={isLoading}
          />
        </div>

        {/* Stats */}
        <div className="mb-6">
          <PropertyStats
            totalProperties={properties.length}
            isLoading={isLoading}
          />
        </div>

        {/* Properties Grid */}
        <PropertyGrid
          properties={properties}
          isLoading={isLoading}
          error={error?.message || null}
          onViewDetails={handleViewDetails}
        />
      </main>

      {/* Footer */}
      <footer className="bg-gray-900 text-white mt-16">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
            <div>
              <h3 className="text-lg font-semibold mb-4">Million Luxury</h3>
              <p className="text-gray-400">
                La mejor experiencia en bienes raíces de lujo.
              </p>
            </div>
            <div>
              <h4 className="text-md font-semibold mb-4">Servicios</h4>
              <ul className="space-y-2 text-gray-400">
                <li>Compra</li>
                <li>Venta</li>
                <li>Alquiler</li>
                <li>Asesoría</li>
              </ul>
            </div>
            <div>
              <h4 className="text-md font-semibold mb-4">Contacto</h4>
              <ul className="space-y-2 text-gray-400">
                <li>+57 300 123 4567</li>
                <li>info@millionluxury.com</li>
                <li>Bogotá, Colombia</li>
              </ul>
            </div>
            <div>
              <h4 className="text-md font-semibold mb-4">Síguenos</h4>
              <ul className="space-y-2 text-gray-400">
                <li>Facebook</li>
                <li>Instagram</li>
                <li>LinkedIn</li>
              </ul>
            </div>
          </div>
          <div className="border-t border-gray-800 pt-8 mt-8 text-center text-gray-400">
            <p>&copy; 2025 Million Luxury. Todos los derechos reservados.</p>
          </div>
        </div>
      </footer>
    </div>
  );
}
