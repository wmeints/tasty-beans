FROM mcr.microsoft.com/mssql-tools

ENV DB_USER ""
ENV DB_PASSWORD ""
ENV DB_SERVER ""
ENV DB_NAME ""

COPY migrate.sh /app/migrate.sh

CMD ["/bin/bash", "/app/migrate.sh"]