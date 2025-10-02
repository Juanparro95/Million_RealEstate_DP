import '@testing-library/jest-dom';
import { renderHook, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactNode } from 'react';
import { useProperties, useProperty, propertyKeys } from '@/hooks/useProperties';
import { PropertyService } from '@/services/property.service';
import { PropertyFilters } from '@/types/property';

// Mock the PropertyService
jest.mock('@/services/property.service');
const mockedPropertyService = jest.mocked(PropertyService);

// Helper to create a query client wrapper
const createWrapper = () => {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        retry: false,
      },
    },
  });

  return ({ children }: { children: ReactNode }) => (
    <QueryClientProvider client={queryClient}>
      {children}
    </QueryClientProvider>
  );
};

describe('useProperties - Technical Test Requirements', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('fetches properties successfully with filters', async () => {
    const mockProperties = [
      {
        id: '1',
        idProperty: 'PROP001',
        name: 'Test Property',
        address: 'Test Address',
        price: 1000000,
        year: 2020,
        ownerName: 'Test Owner',
        mainImage: '/test.jpg'
      }
    ];

    mockedPropertyService.getProperties.mockResolvedValue(mockProperties);

    const filters: PropertyFilters = { name: 'test' };
    const wrapper = createWrapper();

    const { result } = renderHook(() => useProperties(filters), { wrapper });

    await waitFor(() => {
      expect(result.current.isSuccess).toBe(true);
    });

    expect(mockedPropertyService.getProperties).toHaveBeenCalledWith(filters);
    expect(result.current.data).toEqual(mockProperties);
    expect(result.current.isLoading).toBe(false);
    expect(result.current.error).toBe(null);
  });

  it('uses correct query key for properties list', () => {
    const filters: PropertyFilters = { name: 'casa', minPrice: 1000000 };
    const expectedKey = propertyKeys.list(filters);

    expect(expectedKey).toEqual(['properties', 'list', filters]);
  });
});

describe('useProperty - View Property Details', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('fetches property details successfully', async () => {
    const mockProperty = {
      id: '1',
      idProperty: 'PROP001',
      name: 'Test Property',
      address: 'Test Address',
      price: 1000000,
      year: 2020,
      codeInternal: 'CODE001',
      idOwner: 'OWNER001',
      images: [],
      traces: [],
      owner: {
        id: 'owner1',
        idOwner: 'OWNER001',
        name: 'Test Owner',
        address: 'Owner Address',
        photo: '/owner.jpg',
        birthday: '1980-01-01'
      }
    };

    mockedPropertyService.getPropertyById.mockResolvedValue(mockProperty);

    const wrapper = createWrapper();
    const { result } = renderHook(() => useProperty('1'), { wrapper });

    await waitFor(() => {
      expect(result.current.isSuccess).toBe(true);
    });

    expect(mockedPropertyService.getPropertyById).toHaveBeenCalledWith('1');
    expect(result.current.data).toEqual(mockProperty);
    expect(result.current.isLoading).toBe(false);
    expect(result.current.error).toBe(null);
  });

  it('does not fetch when id is empty', () => {
    mockedPropertyService.getPropertyById.mockResolvedValue({} as any);

    const wrapper = createWrapper();
    const { result } = renderHook(() => useProperty(''), { wrapper });

    expect(result.current.status).toBe('pending');
    expect(mockedPropertyService.getPropertyById).not.toHaveBeenCalled();
  });

  it('uses correct query key for property detail', () => {
    const id = '123';
    const expectedKey = propertyKeys.detail(id);

    expect(expectedKey).toEqual(['properties', 'detail', id]);
  });
});

describe('propertyKeys - Query Key Generation', () => {
  it('generates correct query keys', () => {
    expect(propertyKeys.all).toEqual(['properties']);
    expect(propertyKeys.lists()).toEqual(['properties', 'list']);
    expect(propertyKeys.details()).toEqual(['properties', 'detail']);
    
    const filters: PropertyFilters = { name: 'casa' };
    expect(propertyKeys.list(filters)).toEqual(['properties', 'list', filters]);
    
    const id = '123';
    expect(propertyKeys.detail(id)).toEqual(['properties', 'detail', id]);
  });
});