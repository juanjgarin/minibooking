FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/services/CustomerService/CustomerService.Domain/CustomerService.Domain.csproj", "CustomerService/CustomerService.Domain/"]
COPY ["src/services/CustomerService/CustomerService.Application/CustomerService.Application.csproj", "CustomerService/CustomerService.Application/"]
COPY ["src/services/CustomerService/CustomerService.Infrastructure/CustomerService.Infrastructure.csproj", "CustomerService/CustomerService.Infrastructure/"]
COPY ["src/services/CustomerService/CustomerService.Api/CustomerService.Api.csproj", "CustomerService/CustomerService.Api/"]

RUN dotnet restore "CustomerService/CustomerService.Api/CustomerService.Api.csproj"

COPY src/services/CustomerService/CustomerService.Domain/ CustomerService/CustomerService.Domain/
COPY src/services/CustomerService/CustomerService.Application/ CustomerService/CustomerService.Application/
COPY src/services/CustomerService/CustomerService.Infrastructure/ CustomerService/CustomerService.Infrastructure/
COPY src/services/CustomerService/CustomerService.Api/ CustomerService/CustomerService.Api/

WORKDIR /src/CustomerService/CustomerService.Api
RUN dotnet publish "CustomerService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore

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
ENTRYPOINT ["dotnet", "CustomerService.Api.dll"]
