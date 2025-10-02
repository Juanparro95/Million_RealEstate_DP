'use client';

import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { useState } from 'react';

interface QueryProviderProps {
  readonly children: React.ReactNode;
}

export default function QueryProvider({ children }: QueryProviderProps) {
  const [queryClient] = useState(
    () =>
      new QueryClient({
        defaultOptions: {
          queries: {
            // Tiempo antes de que los datos se consideren obsoletos
            staleTime: 60 * 1000, // 1 minuto
            // Tiempo antes de que los datos se eliminen del cache
            gcTime: 5 * 60 * 1000, // 5 minutos (antes era cacheTime en v4)
            // Retry en caso de error
            retry: (failureCount, error: any) => {
              // No hacer retry en errores 404
              if (error?.status === 404) return false;
              // Máximo 3 intentos
              return failureCount < 3;
            },
            // No refetch automático al hacer focus en la ventana
            refetchOnWindowFocus: false,
          },
          mutations: {
            // Retry para mutaciones
            retry: 1,
          },
        },
      })
  );

  return (
    <QueryClientProvider client={queryClient}>
      {children}
      {/* Solo mostrar dev tools en desarrollo */}
      {process.env.NODE_ENV === 'development' && (
        <ReactQueryDevtools initialIsOpen={false} />
      )}
    </QueryClientProvider>
  );
}