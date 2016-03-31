module Gateway

open GitHub
open Http
open ObservableExtensions
open FSharp.Control.Reactive
open System.Reactive.Threading.Tasks


let getProfile username =
  let toRepoWithLanguageStream (repo : GitHubUserRepos.Root) =
    username
    |> languageUrl repo.Name
    |> asyncResponseToObservable
    |> Observable.map (languageResponseToRepoWithLanguages repo)

  let userStream = username |> userUrl |> asyncResponseToObservable

  let popularReposStream = 
    username
    |> reposUrl
    |> asyncResponseToObservable
    |> Observable.map reposResponseToPopularRepos
    |> flatmap2  toRepoWithLanguageStream
  
  async {
    return! popularReposStream
            |> Observable.zip userStream
            |> Observable.map toProfile
            |> TaskObservableExtensions.ToTask
            |> Async.AwaitTask
  }

