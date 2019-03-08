# LayerCoroutine

The story starts with my Game Developer, He's reported to me about a strange thing in Coroutine, about to stop the main coroutine but the coroutine inside isn't even stop.

By default, When you start a coroutine and you have to start another coroutine inside the first coroutine, It should be used the separate coroutine. But if you start a coroutine and yield with another IEnumerator, that IEnumerator should still working with in first coroutine!

So in this example, I've simulate this code to show how it's work.

![](Images/53280738_353162568742565_794517727980027904_n.png)

I've an input on the spacebar button. It's stop the current coroutine activity and start the new one on the ```FirstCoroutine()```

As you see, I've my ```CustomYieldInstruction``` in the ```SecondCoroutine()``` method. It's inside look like this.

![](Images/53792629_2270883872933584_1492382701636812800_n.png)

Yes, It's look like ```WaitForSeconds(float)``` that Unity provide us. But why I didn't use that instead? Because I need to create my ```CustomYieldInstruction``` that using in another purpose (like making AI). This is just simulate the trouble.

The result is, It's working fine!

![](Images/53317875_414314155795748_3144441163373608960_n.png)

I pressed the input to stop the current coroutine activity and start the new one. (Yes, it's mean restart the coroutine) You will see that I've press four times until I let it done.

Yes, I called it **Layering Coroutine** technique. And this example are show as two layering coroutine.

The trouble starts with **three or more** layering coroutine.

![](Images/53383994_428107598010223_1099386334990041088_n.png)

I've put another layer of coroutine named ```ThirdCoroutine()``` and make it layering each other.

And here's result...

![](Images/53749537_2572805316082566_3890496675113861120_n.png)

Unity are not stop the layering coroutine anymore. They let the ```SecondCoroutine``` and ```ThirdCoroutine``` running behind and stop only on the ```FirstCoroutine```.

This is sucks. So I've to dive into native Unity Engine. (Sorry, I can't tell you how...) And take a look at ```CustomYieldInstruction``` and ```YieldInstruction```.

**YieldInstruction**

![](Images/53155093_295221161173558_1939425575231815680_n.png)

**CustomYieldInstruction**

![](Images/53488054_617446868701856_1759987837379280896_n.png)

You see the difference? ```YieldInstruction``` are **not inherit** from anything and labeled with attribute ```[UsedByNativeCode]``` but ```CustomYieldInstruction``` are abstract class that inherit IEnumerator.

That's mean ```CustomYieldInstruction``` are IEnumerator and will working same as you create another IEnumerator to do your purpose. That's not different from doing layering coroutine.

But ```YieldInstruction``` are used in **native, internally and no parameters**.

What is that mean? That's mean Unity Engine, maybe, using ```YieldInstruction``` and do the switch case to execute internally. And we cannot doing anything on ```YieldInstruction```.

Why does I care about using between ```YieldInstruction``` or ```CustomYieldInstruction```? I'll show you right here.

Unity **WaitForSeconds** using ```YieldInstruction```

![](Images/53176723_413156269439418_2418653675317624832_n.png)

Unity **WaitWhile** using ```CustomYieldInstruction```

![](Images/53641355_301002520518911_7006693356159893504_n.png)

And here's different. I've made two test-case between ```WaitForSeconds``` and ```WaitWhile```.

![](Images/53252995_404199940356877_8461153704186216448_n.png)

I've made some changes on the ```ThirdCoroutine()``` to use ```WaitForSeconds```. And here's result.

![](Images/54255709_2119136981511553_8254567834537951232_n.png)

```WaitForSeconds``` are working correctly as **Layering Coroutine**.

So, this time for ```WaitWhile```.

![](Images/53347579_810132339347761_6575346165670215680_n.png)

It's working like a wait for 2 seconds, same as above.

And here's result...

![](Images/53660864_2060911743954766_4850255813744263168_n.png)

They are not **Layering coroutine** anymore!

**WHY? UNITY, WHY!?**

To fix this trouble is report to Unity Developer and hope they will fix this in 2 years later... I'm serious.

So, How to work around on this troble is, use ```LayerCoroutine``` class that I've made. Here's code on the example that I attached to this repo too.

![](Images/54514114_1074520292747135_4803389804920176640_n.png)

Instead of using ```StartCoroutine``` from ```MonoBehaviour```, you need to create ```new LayerCoroutine(MonoBehaviour)``` as a parameter. and use it like ```layer.StartCoroutine(IEnumerator)```.
This will make coroutine that you started are layered inside of ```LayerCoroutine``` class.
And to stop the coroutine, use ```layer.StopAllCoroutine()``` to stop all layering coroutine.

As you see, I'm put another layer called ```FourthCoroutine()``` too. And here's result!

![](Images/52929779_2187670221271996_7951249762261401600_n.png)

Now it's working fine as **Layering Coroutine**.

Thanks for reading!