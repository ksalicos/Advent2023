# Advent2023
Advent Of Code 2023

Day 1: Rejecting regex in part one paid off for part 2.  Going to avoid it in the future.

Day 2: Going to assume inputs are correct, dangerous in the real world but safe here I think.  Lots of .split here, could write a more elegant parser I think.

Day 3: Keeping part one and two instead of replacing so I can reference old code easier.  Cute names were cute, but going to just use day/part.
       Easy once I fixed off-by-one errors.

Day 4: Getting complex enough to use classes.  Part Two was a little weird, but not hard to implement once understood.

Day 5: Having source and destination in the reverse order from what I'd expect added time to this.  Edge cases of splitting ranges added more.
       Needed to be more methodical and write things out before coding.

Day 6: Super easy to make up for yesterday?  Brute force ran fast enough that I didn't look for an optimal solution.  Maybe solve for i*(n1-i)-n2 == 0?

Day 7: Had some trouble getting my head around the proper way to compute the of value hands in part two.  After running against test code from Reddit, I found that I was missing the edge case where jokers were being added to themselves to hit the target number.

Day 8: Now that I've seen solutions, I don't know that I like the LCM version.  I'd rather have code that works without assumptions made about the input.  Still opportunities to speed this up: Chinese remainder theorem?
