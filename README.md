Center-of-Gravity
=================

Generally, this program is an experiment in linear programming.

The goal of this problem is to place a bunch of people and monkeys onto a seesaw so as to optimize the seesaw and its occupants' center of gravity (CG).  Optimizing the CG in this situation means keeping it as close to the seesaw's CG as possible.  Here's the scenario:

Consider an unusually long and likely odd-looking seesaw that is broken up into P positions where each position has a maximum weight capacity, P.w.  There are M monkeys and N people that need to be placed on the seesaw.  Each monkey and person has an associated weight, M.w and N.w.  Each position also has a set of restrictions on the number of monkeys and people that may be seated there.  For example, position 1 might seat a maximum of 2 monkeys and 0 people, or 1 monkey and 1 person.  This means that position 1 can seat the following sets of (m monkeys, n people): {(0, 0), (0, 1), (1, 1), (1, 0), (2, 0)}.  

Each position has a different effect, P.f, on the seesaw's CG.  (Weight placed on the leftmost position will affect the seesaw's CG differently than weight placed on the seesaw's center position.)  To compute the seesaw's CG requires: 1) determining the weight placed at each position, 2) multiplying the total weight at each position by that position's effect on the CG (total weight placed at the position * P.f), 3) summing the numbers computed in step 2 for all positions, and then 4) dividing the number computed in step 3 by the total weight placed on the seesaw.  For a given total weight, there is a middle, "optimal", center of gravity.  Again, the goal is to place the monkeys and people on the seesaw to minimize the distance between the actual center of gravity and the optimal center of gravity.

An example (trivial, no monkeys):
Seesaw S has 2 positions, P1 and P2.
P1 has a maximum weight capacity, P1.w of 300 pounds.
P1 can seat at most 2 people and 0 monkeys.
P1.F = 2
P2 has a maximum weight capacity, P2.w of 300 pounds.
P2 can seat at most 2 people and 0 monkeys.
P2.F = 1
(Weight at P1 has twice the effect on the seesaw's CG than weight at P2.)
2 people need to be seated, N1 and N2.
N1 weighs 150 pounds.
N2 weighs 150 pounds.
The optimal center of gravity (CG) is 1.2.
Where should N1 and N2 be seated?

Possibility #1.
N1 is seated in P1, N2 is seated in P1.
CG = ((150 * 2) + (150 * 2)) / (300) = 2
Distance from optimal CG = Abs(2 - 1.2) = .8

Possibility #2.
N1 is seated in P1, N2 is seated in P2.
CG = ((150 * 2) + (150 * 1)) / (300) = 1.5
Distance from optimal CG = Abs(1.5 - 1.2) = .3

Possibility #3.
N1 is seated in P2, N2 is seated in P1.
CG = ((150 * 1) + (150 * 2)) / (300) = 1.5
Distance from optimal CG = Abs(1.5 - 1.2) = .3

Possibility #4.
N1 is seated in P2, N2 is seated in P2.
CG = ((150 * 1) + (150 * 1)) / (300) = 1
Distance from optimal CG = Abs(1 - 1.2) = .2

The optimal seating is therefore N1 in P2 and N2 in P2.

For a given set of inputs, what's the fastest way to solve this problem?