# Build from MiniBooking repo root.
# Example: docker build -f web/Dockerfile -t mini-booking-web --build-arg VITE_BOOKING_API_URL=http://localhost:5221 --build-arg VITE_CUSTOMER_API_URL=http://localhost:5222 .

FROM node:22-alpine AS build
WORKDIR /app

COPY web/package.json web/package-lock.json ./
RUN npm ci

COPY web/ ./

ARG VITE_BOOKING_API_URL=http://localhost:5221
ARG VITE_CUSTOMER_API_URL=http://localhost:5222
ENV VITE_BOOKING_API_URL=$VITE_BOOKING_API_URL
ENV VITE_CUSTOMER_API_URL=$VITE_CUSTOMER_API_URL

RUN npm run build

FROM nginx:1.27-alpine AS final

COPY web/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD wget -q -O- http://127.0.0.1/ >/dev/null || exit 1
