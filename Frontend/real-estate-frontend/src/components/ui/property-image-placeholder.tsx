import { Building } from "lucide-react";

interface PropertyImagePlaceholderProps {
  readonly name?: string;
  readonly className?: string;
}

export function PropertyImagePlaceholder({ 
  name = "Propiedad", 
  className = "" 
}: PropertyImagePlaceholderProps) {
  return (
    <div className={`w-full h-full bg-gradient-to-br from-blue-100 to-blue-200 flex flex-col items-center justify-center ${className}`}>
      <Building className="h-16 w-16 text-blue-400 mb-2" />
      <span className="text-blue-600 text-sm font-medium text-center px-4">
        {name}
      </span>
    </div>
  );
}