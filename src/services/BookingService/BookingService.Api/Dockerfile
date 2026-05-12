FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/services/BookingService/BookingService.Domain/BookingService.Domain.csproj", "BookingService/BookingService.Domain/"]
COPY ["src/services/BookingService/BookingService.Application/BookingService.Application.csproj", "BookingService/BookingService.Application/"]
COPY ["src/services/BookingService/BookingService.Infrastructure/BookingService.Infrastructure.csproj", "BookingService/BookingService.Infrastructure/"]
COPY ["src/services/BookingService/BookingService.Api/BookingService.Api.csproj", "BookingService/BookingService.Api/"]

RUN dotnet restore "BookingService/BookingService.Api/BookingService.Api.csproj"

COPY src/services/BookingService/BookingService.Domain/ BookingService/BookingService.Domain/
COPY src/services/BookingService/BookingService.Application/ BookingService/BookingService.Application/
COPY src/services/BookingService/BookingService.Infrastructure/ BookingService/BookingService.Infrastructure/
COPY src/services/BookingService/BookingService.Api/ BookingService/BookingService.Api/

WORKDIR /src/BookingService/BookingService.Api
RUN dotnet publish "BookingService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
USER root
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/* \
    && chown -R app:app /app
USER app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "BookingService.Api.dll"]
