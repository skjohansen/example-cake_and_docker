FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src


COPY "WeatherApi/WeatherApi.csproj" .

RUN dotnet restore "WeatherApi.csproj"
COPY WeatherApi/ .
RUN dotnet build "WeatherApi.csproj" --no-restore

FROM build AS publish
RUN dotnet publish "WeatherApi.csproj" --no-restore --no-build -o /out

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /final
COPY --from=publish /out .

ARG USERNAME=empty
ARG APIKEY=empty
RUN echo "Used username: ${USERNAME}"
RUN echo "Used apikey: ${APIKEY}"

ENTRYPOINT ["dotnet", "WeatherApi.dll"]