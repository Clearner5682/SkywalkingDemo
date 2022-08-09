# build images
docker image build -t hongyan5682/authserver:latest -f ./authserver/Dockerfile . 
docker image build -t hongyan5682/base-setting-service:latest -f ./base-setting-service/Dockerfile . 
docker image build -t hongyan5682/gateway:latest -f ./gateway/Dockerfile . 
docker image build -t hongyan5682/identity-service:latest -f ./identity-service/Dockerfile . 
docker image build -t hongyan5682/process-control-service:latest -f ./process-control-service/Dockerfile . 

# remove dangling images
docker rmi $(docker images -f "dangling=true" -q)

# delete remote images from repository
# export HUB_TOKEN=XXXXXXXX
curl -X DELETE -H "Authorization: Bearer $HUB_TOKEN" https://hub.docker.com/v2/repositories/hongyan5682/gateway-web/

# push images
docker login -u hongyan5682 -p 123456Asdcv
docker push hongyan5682/authserver:latest
docker push hongyan5682/base-setting-service:latest
docker push hongyan5682/gateway:latest
docker push hongyan5682/identity-service:latest
docker push hongyan5682/process-control-service:latest