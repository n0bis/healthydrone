#!/bin/sh

postgres_host=$1
postgres_port=$2
portgres_user=$3
shift 2
cmd="$@"

# wait for the postgres docker to be running
while ! pg_isready -h $postgres_host -p $postgres_port -q -U $postgres_user; do
  >&2 echo "Postgres is unavailable - sleeping"
  sleep 1
done

>&2 echo "Postgres is up - executing command"

sleep 5

# run the command
exec $cmd