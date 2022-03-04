docker build -f AuthServer/Dockerfile --force-rm --no-cache -t auth_server .
docker tag auth_server registry.heroku.com/cap-k24-team13-auth/web
docker push registry.heroku.com/cap-k24-team13-auth/web
heroku container:release -a cap-k24-team13-auth web