#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 8080

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY ["rgz.csproj", "./"]
RUN dotnet restore "./rgz.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "rgz.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "rgz.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "rgz.dll"]
