# OPPO_TestRecyclerView
A Xamarin Android project.

## Introduction
This project is Android phone part of the interactive display for OPPO at [2017 Mobile World Congress (MWC) @Barcelona](http://ioiocreative.com/projects/oppo). It contains a [RecyclerView](https://developer.android.com/reference/android/support/v7/widget/RecyclerView) for scrolling a vertical list of [stripped images](#preparation-of-the-vertical-list-of-stripped-images-for-filling-the-recyclerview). It contains a TCP server. The scrolling speed and scrolled amount of the RecylcerView are sent to a TCP client connected to the phone either via Wifi or a USB cable, through [ADB port forwarding](#adb-port-forwarding-for-tcp-server-client-connection-using-ports-of-the-same-machine). Note the TCP client is not part of this project.
**2022-03-15 update**
The original *android.support.v7.widget* package has been migrated and replaced by *andoirdx.recyclerview.widget* and the target Android Framework has been updated to Android 10 - API 29


## Partner repositories
This project works with either one of the following repositories, which acts as a TCP client.
* [OPPO_TcpClientThread](https://github.com/ioio-creative/OPPO_TcpClientThread) - a TCP client C#.NET Windows Form program for testing messages received from the phone server.
* [OPPO_ImageSequence](https://github.com/ioio-creative/OPPO_ImageSequence) - a TCP client OpenFrameworks program for production use in the MWC OPPO interactive display.

## Preparation of the vertical list of stripped images for filling the RecyclerView
Originally, we can have a 1080 x (1920 x n), (or anything that is in accordance to the aspect ratio and/or resolution of the phone), bottom-and-top-continuous image. Then we can divide this original image into a vertical list of stripped images by running the following [Image Magick](https://imagemagick.org/index.php) command for [tile cropping / sub-dividing one image into multiple images](http://www.imagemagick.org/Usage/crop/#crop_tile).
```
$ magick convert phoneimage.jpg -crop 1080x48 +repage +adjoin phoneimage_1080x48_%05d.jpg
```
The magic number 48 in the command is because of the fact that 1920 is divisible by 48. Therefore, the above command divides a 1080 x 1920 image into fifty 1080 x 48 images.

## ADB port forwarding for TCP server-client connection using ports of the "same" machine
To connect a TCP client of [OPPO_TcpClientThread](#partner-repositories) to the TCP server run in this phone's program, one has to run the following [ADB port forwarding](https://blog.usejournal.com/adb-port-forwarding-and-reversing-d2bc71835d43) command on the machine which the phone is connected via [ADB](https://developer.android.com/studio/command-line/adb).
```
$ adb forward tcp:12580 tcp:10086
```

On the other hand, [OPPO_ImageSequence](#partner-repositories) is programmed to run the above ADB port forwarding command itself.