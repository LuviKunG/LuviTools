# Enchant List
Some useful list that didn't exist in .NET Framework

## Loop List
Like as list but easier to pick element by order and will automatic loop.
This one using for Pool scripts

If you want to get current T, use this.

    T GetCurrent()
	
If you want to get next of element, use this.

    T GetNext()

## Limit List
Like as list but it's have limit number. If add new element into the top and it's size reach to limit, it will remove the old one from bottom.
This one is using as list of string for debugger.

This will overwrite the old method of Add(T elem) of List.

    void Add(T elem)
	
## Sector List
Like as list but it's have 'Next' member to check that can pick a next element or not.

If you want to check that can get next element, use this.

    bool Next
	
If you want to reset, use this.

    void Reset()
	
If you want to get next of element, use this.

    T GetNext()