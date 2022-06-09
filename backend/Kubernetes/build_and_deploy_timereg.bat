cd D:\udv\paw\backend\TimeregService
docker build -t dkmoelgaard75/timeregservice -f "TimeregService/Dockerfile" .
docker push dkmoelgaard75/timeregservice

cd D:\udv\paw\backend\Kubernetes\
kubectl apply -f deploy-timereg.yaml
kubectl rollout restart deployment timereg-depl
pause