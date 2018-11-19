FROM microsoft/aspnetcore-build:lts
COPY . /webrgz-master
WORKDIR /webrgz-master
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
EXPOSE 80/tcp
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh