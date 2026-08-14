FROM node:22-alpine AS build
WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci

COPY . .
RUN npm run build

FROM nginx:alpine AS runtime

# Alvo do proxy /api — passe sempre no `docker run -e` / nas env vars do
# Container App, apontando para a URL da API
# (ex.: https://inputweb-api.<regiao>.azurecontainerapps.io).
ENV API_URL=http://host.docker.internal:5143
# Impede o envsubst de comer as variáveis do nginx ($uri, $host, ...).
ENV NGINX_ENVSUBST_FILTER=^API_URL

COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf.template /etc/nginx/templates/default.conf.template

EXPOSE 80
