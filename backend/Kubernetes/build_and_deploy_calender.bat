cd D:\udv\paw\backend\CalendarService\
docker build -t dkmoelgaard75/calendarservice -f "CalendarService/Dockerfile" .
docker push dkmoelgaard75/calendarservice

cd D:\udv\paw\backend\Kubernetes\
kubectl apply -f deploy-calendar.yaml
kubectl rollout restart deployment calendar-depl
pause