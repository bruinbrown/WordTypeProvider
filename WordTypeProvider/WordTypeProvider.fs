namespace WordTypeProvider

open Microsoft.FSharp.Core.CompilerServices
open Microsoft.FSharp.Quotations
open ProviderImplementation.ProvidedTypes
open System
open System.Reflection

[<TypeProvider>]
type WordTypeProvider(cfg:TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces()

    let baseType = Some typeof<obj>
    let ns = "Word.TypeProvider"
    let asm = Assembly.GetExecutingAssembly()
    let alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

    let rec createTypes wordSoFar =
        alphabet
        |> Seq.map (fun t -> let newChar = t |> string
                             let newWord = wordSoFar + newChar
                             let ty = ProvidedTypeDefinition(newChar, None)
                             ty.AddMembersDelayed(fun () -> createTypes newWord)
                             let wordProp = ProvidedProperty("Word", typeof<string>, IsStatic=true, GetterCode = fun args -> <@@ newWord @@>)
                             ty.AddMember(wordProp)
                             ty)
        |> Seq.toList

    let rootType = ProvidedTypeDefinition(asm, ns, "StartHere", None)
    do rootType.AddMembersDelayed(fun () -> createTypes "")
    do this.AddNamespace(ns, [rootType])

[<assembly:TypeProviderAssembly>]
do ()
