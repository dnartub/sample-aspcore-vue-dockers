FROM node:latest as build
WORKDIR /src

COPY . .

RUN npm install

RUN npm rebuild node-sass

RUN npx vue-cli-service build --mode staging

FROM nginx as final
COPY --from=build /src/dist/ /app-front/
