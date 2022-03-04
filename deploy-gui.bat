docker build -f GUI/Dockerfile --force-rm --no-cache -t gui .
docker tag gui registry.heroku.com/cap-k24-team13/web
docker push registry.heroku.com/cap-k24-team13/web
heroku container:release -a cap-k24-team13 web