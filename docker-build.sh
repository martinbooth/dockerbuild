docker run \
    --rm
    -v $(which docker):/usr/local/bin/docker \
    -v /var/run/docker.sock:/var/run/docker.sock \
    -v $(pwd):/var/app \
    -e "TEAMCITY_VCSROOT_BRANCH=$TEAMCITY_VCSROOT_BRANCH" -e "BUILD_VCS_NUMBER=$BUILD_VCS_NUMBER" -e "TEAMCITY_VERSION=$TEAMCITY_VERSION" -e "BUILD_NUMBER=$BUILD_NUMBER" \
    mono /var/app/build.sh "$@"
