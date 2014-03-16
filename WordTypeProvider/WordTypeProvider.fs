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

    let rootType = ProvidedTypeDefinition(asm, ns, "StartHere", None)
    do this.AddNamespace(ns, [rootType])

[<assembly:TypeProviderAssembly>]
do ()
