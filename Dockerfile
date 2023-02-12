
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Payment.Api/**.csproj ./Payment.Api/
COPY Payment.Domain/**.csproj ./Payment.Domain/
COPY Payment.Persistence/**.csproj ./Payment.Persistence/
COPY Payment.Application/**.csproj ./Payment.Application/
COPY Payment.Banks/**.csproj ./Payment.Banks/

WORKDIR /app/Payment.Api
RUN dotnet restore

# Copy everything else and build
COPY . /app
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/Payment.Api/out .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "Payment.Api.dll"]