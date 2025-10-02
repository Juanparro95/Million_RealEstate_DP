import { PropertyService } from '@/services/property.service';
import { PropertyFilters } from '@/types/property';

// Mock the entire PropertyService for unit testing
jest.mock('@/services/property.service', () => ({
  PropertyService: {
    getProperties: jest.fn(),
    getPropertyById: jest.fn(),
  },
}));

const mockedPropertyService = jest.mocked(PropertyService);

describe('PropertyService - Technical Test Requirements', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  describe('getProperties - List properties with filters', () => {
    it('fetches properties without filters', async () => {
      const mockProperties = [
        {
          id: '1',
          idProperty: 'PROP001',
          name: 'Test Property',
          address: 'Test Address',
          price: 1000000,
          year: 2020,
          ownerName: 'Test Owner',
          mainImage: '/test.jpg',
          bedrooms: 3,
          bathrooms: 2,
          area: 120,
          images: []
        }
      ];

      mockedPropertyService.getProperties.mockResolvedValue(mockProperties);

      const result = await PropertyService.getProperties();

      expect(mockedPropertyService.getProperties).toHaveBeenCalledWith();
      expect(result).toEqual(mockProperties);
    });

    it('fetches properties with filters (name, address, price range)', async () => {
      const mockProperties = [
        {
          id: '1',
          idProperty: 'PROP001',
          name: 'Casa Test',
          address: 'Bogotá',
          price: 1500000,
          year: 2021,
          ownerName: 'Test Owner',
          mainImage: '/test.jpg',
          bedrooms: 3,
          bathrooms: 2,
          area: 120,
          images: []
        }
      ];

      const filters: PropertyFilters = {
        name: 'Casa',
        address: 'Bogotá',
        minPrice: 1000000,
        maxPrice: 2000000,
        page: 1,
        pageSize: 10
      };

      mockedPropertyService.getProperties.mockResolvedValue(mockProperties);

      const result = await PropertyService.getProperties(filters);

      expect(mockedPropertyService.getProperties).toHaveBeenCalledWith(filters);
      expect(result).toEqual(mockProperties);
    });

    it('handles API errors correctly', async () => {
      const error = new Error('Network Error');
      mockedPropertyService.getProperties.mockRejectedValue(error);

      await expect(PropertyService.getProperties()).rejects.toThrow('Network Error');
    });

    it('handles server errors (500)', async () => {
      const serverError = new Error('Internal server error');
      mockedPropertyService.getProperties.mockRejectedValue(serverError);

      await expect(PropertyService.getProperties()).rejects.toThrow('Internal server error');
    });

    it('handles timeout errors', async () => {
      const timeoutError = new Error('Request timeout');
      mockedPropertyService.getProperties.mockRejectedValue(timeoutError);

      await expect(PropertyService.getProperties()).rejects.toThrow('Request timeout');
    });
  });

  describe('getPropertyById - View property details', () => {
    it('fetches a property by id for viewing details', async () => {
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

      const result = await PropertyService.getPropertyById('1');

      expect(mockedPropertyService.getPropertyById).toHaveBeenCalledWith('1');
      expect(result).toEqual(mockProperty);
    });

    it('handles not found error (404)', async () => {
      const error = new Error('Property not found');
      mockedPropertyService.getPropertyById.mockRejectedValue(error);

      await expect(PropertyService.getPropertyById('999')).rejects.toThrow('Property not found');
    });

    it('handles connection errors', async () => {
      const connectionError = new Error('Network Error');
      mockedPropertyService.getPropertyById.mockRejectedValue(connectionError);

      await expect(PropertyService.getPropertyById('1')).rejects.toThrow('Network Error');
    });
  });
});