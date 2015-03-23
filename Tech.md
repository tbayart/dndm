# How does this help me? #

.Net Download Manager is a download manager (duh!). Download managers help boost your download speed by making multiple connections to the same host. **There is no magic here, it doesn't make your internet faster.** Let me repeat that in case you didn't catch it - download managers aren't mystical pieces of software that make bandwidth out of nothing.

Technically it downloads each part in a separate thread, which the parallelization of the parts should be faster than using a single thread.

The parallelization of the parts would also be useful if you have multiple internet connections (cell phone, DSL, cable provider ect ect). This should work great with home built setups or software like http://www.connectify.me/dispatch/

With some internet providers they will actually throttle the speed of a connected client. Using segmented downloads will help in that situation.

But how much faster? Many "download managers" will make some wild claims, but in reality it will vary.

DNDM also provides the ability to pause and resume downloads if the server supports it.