# ChessGame

## Building and run docker image
``` docker build -t chess-image -f Dockerfile . ```

``` docker create --name chess-container -p 5050:80  chess-image ```

```  docker start chess-container ```

Open http://localhost:5050 and enjoy :)

