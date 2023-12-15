# Advent2023
[Advent Of Code 2023](https://adventofcode.com/2023)

I'm trying to approach this from a standpoint of what I would accept in a code review.  I'm wavering on the idea of treating the input as a test case or as the only goal.  I'd like to treat the problem text as the only requirements, but have strayed from that on occasion.
A general assumption is that the input has already been checked for correctness, IRL I would have more error checking.

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

Day 9: Brute force was fast and easy, didn't see an obvious algorithm to get cute.

Day 10: Part one was fairly straightforward.  I skipped writing code that would determine valid starting directions, since the input was easy to read.  Part two was a matter of using the correct algorithm.  Initially, I wanted to use a pathing algorithm - anything I couldn't path to from an outside edge must be inside.
        This would have been fine, except that I couldn't find an elegant solution to pathing through the adjacent pipes.  After failing at this for a while, I remembered the fill algorithm I used.  This got close, but it took me a while to realize it needed to be modified to take into account the direction of the pipe.
        Leaving the map drawing in because it's pretty.

Day 11: Super trivial.  On part one, I actually altered the map, but looking at part two I changed it to a more elegant solution.  Used extra time to clean up old entries.

Day 12: Not Super Trivial.  Going to check in the multiple algorithms I tried, to look over later and see where I went wrong.  Part 2 beat me.  Even with what should have been a faster algorithm, my stack grew too quickly.

Day 13: Super trivial.  Day 12 had trained me well for this.

Day 14: Took too long after misreading part two.  I wrote a method for calculating part one without modifying the input, but then found I did need to modify the input for part two.
Code is wonky due to trying to solve for the wrong answer.  I did think of a way to do the transforms in one pass, but this ran fast enough that optimization wasn't needed.

Day 15: The problem was simple, however I apparently need a refresher on basic arithmatic.  Luckily, unit tests saved me from committing a very embarassing error!
