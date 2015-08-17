# Demonstration of building and creating docker container from within a docker container

This is a demonstration of an F# FAKE build script for building a basic C# self-hosting webapi project.

It can be ran in 3 different ways:
- build.cmd on windows. Note that you need to have the docker client, and the environment variables configured so that this works. You'll probably need to install boot2docker and run boot2docker shellinit | iex, or docker toolbox and use docker-machine env {machinename} | iex
- build.sh on linux. Note, this will need mono and docker installed
- docker-build.sh. This will only need docker installed. It will run build.sh inside a docker container which has the necessary prerequistes installed (in this case, just mono).

### Purpose

The purpose of this demonstration is to show how it is possible to build your source code on a build agent with nothing installed except docker itself and the build agent service.

If you see the value of using docker containers to run your software in production then you'll probably understand the benefits of using docker containers to build your software too.

The official mono docker container is used to build this sample application because the only prerequisite required to build this simple application is mono itself. For more complicated applications it is recommended that you create a purpose-built container for running your build so that you can include all prerequisites.

### docker-build.sh

A typical build script involves compilation, testing and (if you're already looking at docker) it probably ends in generating a docker container itself.

Running a command inside a docker container it pretty easy; it will typically look like this:

``` docker run -v $(pwd):/var/app mono /var/app/build.sh ```

This will run build.sh inside a container with mono installed (this container is built and supplied courtesy of the mono team themselves). The option: ```-v $(pwd):/var/app``` maps the current path to the directory /var/app within the container so that the full source code is available from within the container.

This is not quite sufficient to run the demonstration build in this repo. The final steps of this build (build.fsx) generate and upload a docker container for deploying to a production webserver however the mono container doesn't contain docker itself, therefore the final steps would normally fail.

In order to get around this, docker-build.sh maps a couple more things from the host into the build container: the docker exectuable itself; $(which docker) and the docker.sock file; required so that the docker client can communicate with the docker server component running on the host.

### DooD vs DinD

Docker outside of docker vs docker inside docker.

Did you know that its possible to install docker inside a docker container? If you need to use docker itself from within a container (such as when we're building docker containers in this example) you _could_ install docker into your build container instead of mapping in the host's executable and connecting to the host's docker service.

If you use the [DinD](https://github.com/jpetazzo/dind) approach, you will not be able to benefit from the caching of layers that you get by using the host's docker service. You'll also need to create your own docker container to build in vs using the existing ones (such as the mono container in this example). Finally, your build container will be larger because it includes docker client+service, which is guaranteed to be installed on the host anyway.

### Host Docker Executable

```-v $(which docker):/usr/local/bin/docker``` maps the host's docker executable into the build container. Please note that not every linux distribution has an executable that is designed to be portable (i.e. works well inside docker containers). It might be dynamically linked with local libraries, or it might require a specific version of libc. If you face this problem (i.e. you have an error running your build because the docker executable will not run within your build container) then replace the docker executable on the host with one from here: https://docs.docker.com/installation/binaries/ They will work everywhere



