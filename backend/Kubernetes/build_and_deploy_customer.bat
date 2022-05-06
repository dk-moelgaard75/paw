cd D:\udv\paw\backend\CustomerService\
docker build -t dkmoelgaard75/customerservice -f "CustomerService/Dockerfile" .
docker push dkmoelgaard75/customerservice

cd D:\udv\paw\backend\Kubernetes\
kubectl apply -f deploy-customer.yaml
kubectl rollout restart deployment customer-depl
pause