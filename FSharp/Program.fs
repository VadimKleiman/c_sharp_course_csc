// Learn more about F# at http://fsharp.org

open System

//1
let rec helper f s n = if n > 0 then helper (f + s) f (n - 1) else f

let fib n = helper 0I 1I n

//2
let rec reverse l = 
    let rec helper acc l = 
        match (acc, l) with
        |   (acc, []) -> acc
        |   (acc, x::xs) -> helper (x::acc) xs
    helper [] l

//3
let halve xs =
    let n = List.length xs / 2
    (List.take n xs, List.skip n xs)

let rec merge xs ys = 
    match (xs,ys) with
    |   [], ys -> ys
    |   xs, [] -> xs
    |   (x::xs), (y::ys) -> if x < y then x::merge xs (y::ys) else y::merge (x::xs) ys

let rec mergesort l = 
    match l with
    |   [] -> []
    |   [x] -> [x]
    |   xs -> let l, r = halve xs
              merge (mergesort l) (mergesort r)

//4

type MultiTree =
    | Leaf of float
    | Add of MultiTree * MultiTree
    | Div of MultiTree * MultiTree
    | Sub of MultiTree * MultiTree
    | Pow of MultiTree * MultiTree
    | Mul of MultiTree * MultiTree

let rec exec (t : MultiTree) = 
    match t with
    |   Leaf x -> x
    |   Add (l, r) -> exec l + exec r
    |   Div (l, r) -> exec l / exec r
    |   Sub (l, r) -> exec l - exec r
    |   Mul (l, r) -> exec l * exec r
    |   Pow (l, r) -> exec l ** exec r
 
//5
let isPrime n =
    let rec check i = i > n/2 || (n % i <> 0 && check (i + 1))
    check 2

let prime = Seq.initInfinite ((+) 2) |> Seq.filter isPrime



[<EntryPoint>]
let main argv =
    let genRand =
        let rand = System.Random()
        fun () -> rand.Next(-100, 100)
    printfn "Fibonacci: %A" <| fib 99
    printfn "Reverse: %A" <| reverse [1 .. 10]
    printfn "Merge Sort: %A" <| mergesort [ for i in 1..100 -> genRand() ]
    //(-1.0 + 5.0) / (2.0 ^ 2.0) * 2.0
    printfn "(-1.0 + 5.0) / (2.0 ^ 2.0) * 2.0 = %A" <| exec (Mul(Div(Add(Leaf -1.0, Leaf 5.0), Pow(Leaf 2.0, Leaf 2.0)), Leaf 2.0))
    printfn "Prime: %A" prime
    0