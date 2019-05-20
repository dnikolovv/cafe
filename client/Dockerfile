FROM node:12.2.0-alpine

WORKDIR /app

COPY . .

ENV PATH /app/node_modules/.bin:$PATH

COPY package.json /app/package.json
RUN npm install
RUN npm install react-scripts@3.0.1 -g

ARG REACT_APP_ENVIRONMENT

RUN npm run build:${REACT_APP_ENVIRONMENT}

RUN npm install -g serve

CMD ["serve", "-s", "build", "-l", "3000"]