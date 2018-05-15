using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGameSpace
{
    class Story
    {
  //      public static AgendaItem NIL = new AgendaItem("Not yet programmed", RiskId.Generic, "NN", Option.CloseAgendaItem("Damn."));

        public static void CreateStory(Session s)
        {

            // 2018
            s.Items.Add(new AgendaItem("Introduction", RiskId.Generic, @"In October 2017, a hitherto unknown terrorist group seized an Israeli nuclear weapon and transported it to Baghdad. It detonated and the city was destroyed.

The peoples of the world were overcome with fear. They ran to their governments in search of help and safety and the governments answered. In a unanimous decision of United Nations members, a new UN specialized agency was founded: the Existential Risk Prevention Authority and you are appointed as its first director-general.

Your mandate is to identify ""existential risks"" -- possible events that could cripple or destroy human civilization -- and to prevent them. Among your first goals should be to reduce the threat posed by nuclear weapons which currently appear to be the most probable cause of human extinction but when you deal with them, your work only begins.

The world has faith in you -- for now. Do not fail the world.


(Gameplay help is available in the left information bar.)", Option.CloseAgendaItem()));

            Story.NuclearStory(s);
            Story.RedHerrings(s);
            Story.AsteroidStory(s);
            Story.FundingStory(s);
            Story.PandemicStory(s);
        }

        private static void PandemicStory(Session s)
        {
            var pandemicRiskIntro = new Risk(RiskId.Pandemic, "Pandemic", "A global epidemic of a virulent and contagious disease could kill milliards. While there are no known pathogens that would penetrate the strictest precautions and we can reasonably expect a portion of the population to be immune or to survive the disease, such a death toll could still cause the collapse of society and will count as a game over.").IntroductionAgendaItem();
            var farmingStandards = new AgendaItem("Farming Standards", RiskId.Pandemic, "Farms are breeding grounds for epidemics. The overuse of antibiotics in farm animals leads to antibiotic-resistant strains of bacteria. These so-called 'superbugs' are almost impossible to treat and if a contagious and virulent one emerges, it could signal the end of humankind. This could happen at any time.\n\nFor these reasons, in light of the possible consequences, the United Nations recommend that we issue a worldwide ban on their use in the hope that will prevent the rise of superbugs.", 
                 Option.AnalyzeIssue(1,1,"This would give us a good chance of preventing a pandemic, but it would make the United States who rely on their farming industry and use antibiotics a lot, rather unhappy."),
                 Option.TimedWork("Implement the ban.", 2, 3, (ai, ss) =>
                 {
                     ss.US.WorsenAttitude(3);
                     ss.UN.ImproveAttitude(1);
                     ss.Flag_BanImplemented = true;
                     ss.TriggerInfo("We have implemented a worldwide ban on the use antiobiotics in farm animals. This has made the United States extremely unhappy with our agency but the ban is in place.", "Let's hope it's enough");
                 }),
                 Option.Delay(),
                 Option.CloseAgendaItem("No. That is too drastic."));
            var bioresearchStandards = new AgendaItem("Bioresearch Standards", RiskId.Pandemic, "An ordinary disease might become a pandemic threat but we should be even more concerned about a bioengineered disease. With the technology available this decade, it might be finally possible to create a disease more dangerous than any before.\n\nEvolution is blind and incapable of creating a perfect pathogen. However, people who intend to harm are not. The European Union and Japan strongly urge us to regulate bioresearch to prevent this.",
                Option.AnalyzeIssue(1,1,"An engineered disease would be a serious threat. While no-one has been able to create one up to now, no-one has seriously tried, and we should keep it that way. Subjecting research to strict supervision and regulation would undoubtedly slow down or eliminate possible advances in creating these pathogens."),
                Option.TimedWork("Train EU and JP institutions as regulators",3, 8, (ai, ss) =>
                {
                    ss.TriggerInfo("You have offloaded bioresearch regulation work to regulatory bodies within the European Union and Japan who gladly accepted the work, even if they were disappointed by the lack of financial support from the UN.\n\nStill, there are now agencies whose sole purpose is regulating bioresearch worldwide and are now overseeing all such research. It is unlikely somebody could be seriously working on a dangerous pathogen without their oversight. The risk of a bioengineered plague is much less now.");
                    ss.Flag_BioReady = true;
                }),
                Option.TimedWork("Regulate research yourself", 6, 3, (ai, ss) =>
                {
                    ss.TriggerInfo("We have established a respectable UN agency for regulating bioresearch worldwide and are now overseeing all such research. It is unlikely somebody could be seriously working on a dangerous pathogen without our oversight. The risk of a bioengineered plague is much less now.\n\nThe EU and Japan are very grateful to us.");
                    ss.EU.ImproveAttitude(2);
                    ss.JP.ImproveAttitude(2);
                    ss.Flag_BioReady = true;
                }),
                Option.Delay(),
                Option.CloseAgendaItem("Research should be free - do nothing."));
            s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                ss.Trigger(pandemicRiskIntro);
                ss.Trigger(farmingStandards);
            }, 4));
            s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                ss.Items.Add(bioresearchStandards);
            }, 12));
            s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                if (!ss.Flag_BanImplemented)
                {
                    ss.GameOver("A superbug emerged from a Chinese province with unprecedented contagiousness and virulence. It spread from city to city until all nations of the world were stricken with the disease. Milliards died and society collapsed around them.");
                }
            }, 10));
            s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                if (!ss.Flag_BioReady)
                {
                    ss.GameOver("An engineered plague came about without warning. Remnants of the terrorist organization responsible for the Baghdad attack improved upon a publically available research and created a pathogen by combining traits of several existing ones, resulting in a deadly contagious disease that could not be cured. Milliards died and society collapsed.");
                }
            }, 21));
        }

        private static void RedHerrings(Session s)
        {
            var reverse = new AgendaItem("Magnetic Reversal", RiskId.RedHerring,
                "A magnetic reversal is an event during which the magnetic north and south exchange positions. A group of Japanese scientists believe that another one may be imminent and that it may have catastrophic consequences, up to mass extentinction and the destruction of Earth's magnetic field protecting the planet from high energy particle streams from the Sun. They believe that with funding, they could learn more about both the probability of this event and about possible ways to counteract its negative effects.",
                Option.AnalyzeIssue(2, 1, "There is historical evidence for brief complete magnetic reversals. During the last such event, the strength of the magnetic field weakened to 5% of its present strength. However, the scientific consensus appears to be that this will not materially affect humans on this planet and the event can be ignored."),
                Option.TimedWork("Do this research", 3, 8, (ai, ss) =>
                {
                    ss.TriggerInfo("Magnetical Reversal research did not result in any indication that magnetic reversal is imminent. Apparently, even if it were, its dangers were grossly overstated by the scientists.", "At least I can rest easy");

                }),
                Option.Delay(),
                Option.CloseAgendaItem("Nonsense."))
               ;
            var blackholes = new AgendaItem("Rogue Black Holes", RiskId.RedHerring,
                "{i}Rogue black holes{/i} are small black holes that, unlike supermassive blackholes at the centers of most galaxies, are not attached gravitationally to any other object. If one wanders near the solar system, its gravity could perturb the orbits of Earth, changing its seasons too hot or too cold for humans to survive. Since they emit no light, rogue black holes are really hard to detect.\n\nA group of European scientists asks you to divert the world's attention to the study of these black holes so that we might be ready for a possible encounter with them.",

                 Option.AnalyzeIssue(1, 1, "No rogue black holes have been detected thus far. With additional funding, some might be detected, but the chances are low, and even if they were detected, there is little we can do about it with our technology."),
                Option.TimedWork("Spotlight this research", 2, 4, (ai, ss) =>
                {
                    ss.TriggerInfo("The results of the rogue black hole research were inconclusive and no rogue black holes have been detected.", "Let's hope they won't come");

                }),
                Option.Delay(),
                Option.CloseAgendaItem("Don't do this."));

            s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                ss.Items.Add(reverse);
            }, 5));
            s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                ss.Items.Add(blackholes);
            }, 14));
        }

        private static void NuclearStory(Session s)
        {
            var nuclearKorea = new AgendaItem("Nuclear Korea", RiskId.NuclearWeapons,
                "North Korea's nuclear weapons have become powerful and numerous. Other countries of the world only have blueprints, no functional weapons and the future is uncertain without the threat of nuclear deterrence. Now North Korea issued an ultimatum: South Korean government must submit to the rule of North Korean leaders and accept a Korean reunification, or else it will be utterly destroyed.",
                Option.TimedWork("This regional dispute does not concern us.", 0, 2, (ai, ss) =>
                {
                    ss.Trigger(new AgendaItem("Seoul destroyed", RiskId.NuclearWeapons, "North Korea followed through on its ultimatum and annihilated the South Korean capital with nuclear weapons. The world's response was swift. Conventional armies rushed to the Korean peninsula. Special forces operatives assassinated North Korean leaders and the country was soon occupied by a United Nations alliance.\n\nBut damage has been done. The world's failure to protect the citizens of South Korea reflected badly even upon you and your standing among the peoples of the world fell.\n\nAt least the world is now finally free of nuclear weapons.", Option.CloseAgendaItem("Regrettable.")));
                    ss.UN.WorsenAttitude(1);
                    ss.EU.WorsenAttitude(2);
                    ss.US.WorsenAttitude(2);
                    ss.JP.WorsenAttitude(2);
                }, true),
                Option.TimedWork("Deescalate tensions.", 2, 8, (ai, ss) =>
                {
                    ss.TriggerInfo("It took a long time and the efforts of the world's best diplomats, but you have managed to deescalate tensions on the Korean peninsula and even convinced North Korea to give up its nuclear weapons and submit to an inspection regime. The world is finally free of nuclear weapons.");
                }),
                Option.TimedWork("Coordinate a preemptive conventional attack.", 1, 2, (ai, ss) =>
                {
                    if (ss.JP.Attitude >= Attitude.CloseFriendship)
                    {
                        ss.Trigger(new AgendaItem("World free of nuclear weapons", RiskId.NuclearWeapons, "You advised the world's leaders on a plan for a preemptive strike.\n\nConventional armies rushed to the Korean peninsula. Special forces operatives assassinated North Korean leaders and the country was soon occupied by a United Nations alliance.\n\nNuclear weapons owned by the regime have been found, confiscated and safely destroyed. Finally, the world is free of nuclear weapons. The United Nations are eternally grateful.", Option.CloseAgendaItem("Phew.")));
                        ss.UN.ImproveAttitude(4);
                    }
                    else
                    {
                        ss.Trigger(new AgendaItem("Korean situation resolved", RiskId.NuclearWeapons, "You attempted to advise the world nations but Japan did not trust your advice and put forth a plan that was needlessly risky.\n\nConventional armies rushed to the Korean peninsula. Special forces operatives assassinated North Korean leaders and the country was soon occupied by a United Nations alliance.\n\nHowever, North Korean military succeeded in firing off a nuclear missile that destroyed a minor South Korean city. You believe that with a better plan, this could have been avoided. Perhaps if your relationship with Japan was better?\n\nAt any rate, at least now the world is finally rid of nuclear weapons.", Option.CloseAgendaItem("Understood")));
                        ss.JP.WorsenAttitude(1);
                    }
                }));

            s.Items.Add(new Risk(RiskId.NuclearWeapons, "Nuclear Weapons", "Nuclear weapons are weapons of mass destruction. Enough weapons exist that, if they were activated by their owners, all humans living on the planet might be annihilated. As long as they exist, the risk of an immediate end for the world is always here. If enough nuclear weapons are used, it is possible that self-perpetuating firestorms will form that will blacken the atmosphere with soot, resulting in a so-called {i}nuclear winter{/i} blotting out the sun and drastically cooling Earth. This may be apocalyptic.", Assessment.RecognizedRisk, Assessment.RecognizedRisk, Assessment.TopPriority, Assessment.RecognizedRisk).IntroductionAgendaItem());

            s.Items.Add(new AgendaItem("Attitude Towards Nuclear Weapons", RiskId.NuclearWeapons, @"After the Baghdad nuclear terrorist attack of 2017, the world is more than ever ready to seriously implement a ban on the use and production of nuclear weapons and to consider destroying these weapons altogether. A nuclear exchange between the world's most powerful countries could destroy the vast majority of human population which - even if not all humans are killed - counts as an existential risk under your purview.

UN: We have always stood for a world without nuclear weapons. Now is the time for their total elimination.
US: Nuclear weapons - in the hands of trustworthy actors - are the best defense against those who would wish to destroy us or the world.
North Korea: We won't give up nuclear weapons no matter what you say so do whatever.",
             Option.AnalyzeIssue(1, 1, "Doing nothing is risky because the threat of a nuclear war or theft remains. In this climate, most countries would agree to total destruction and we can deal with North Korea separately. Passing weapons to the UN will alienate the UN a little because they believe we can do better and North Korea will still not agree, but the US will be more cooperative and it the risk with the UN holding the weapons is much less than if states have it."),
              Option.TimedWork("Ban existence of nuclear weapons", 2, 1, (ai, ss) =>
              {
                  ss.Flag_WeaponsDestroyed = true;
                  ss.UN.ImproveAttitude(1);
                  ss.US.WorsenAttitude(1);
                  ss.TriggerInfo("UN: The UN welcomes this day when the international community finally decided to immediately destroy all nuclear weapons in existence and to keep the secret of their creation safely hidden. The risk of an apocalypse is much less now. North Korea's reluctance to submit to Existential Risk Prevention Authority's edict is regrettable but will not deter the world's nations from doing what is right.");
                  ss.FutureAgenda.Add(new DelayedAgendaItem(null, (a4, s4) => s4.Items.Add(nuclearKorea), 3));
              }),
              Option.TimedWork("Transfer ownership of nuclear weapons to the UN", 1, 1, (ai, ss) =>
              {
                  ss.US.ImproveAttitude(1);
                  ss.TriggerInfo("UN: The creation of the UN Nuclear Weapons Ownership Agency is a step towards the elimination of nuclear weapons. While we regret that the world has not used this opportunity to rid itself of these weapons forever, the weapons are now at least in the hand of the Security Council and it will be much more difficult to trigger a nuclear exchange that would doom this world. We can now all rest a little easier, despite North Korea's reluctance to do what is right.");
                  ss.FutureAgenda.Add(new DelayedAgendaItem(null, (a4, s4) => s4.Items.Add(nuclearKorea), 3));
              }),
              Option.TimedWork("Do nothing", 0, 0, (ai, ss) =>
              {
                  ss.FutureAgenda.Add(new DelayedAgendaItem(null, (aii, sss) =>
                  {
                      sss.Trigger(new AgendaItem("Portents of Nuclear War", RiskId.NuclearWeapons,
                          "Tensions over matters of little importance have escalated between the world's major powers. The terror of the Baghdad attack subsided and countries are more reluctant now to give up their armaments. The United States and some European Union countries are both claiming the need of these weapons for self-defense against rogue actors, and Japan also considers gaining access to these weapons.\n\nSeveral countries have renovated and reactivated their defense mechanisms and ready to fire weapons at a moment's notice.",
                          Option.TimedWork("Do nothing", 0, 1, (ai4, ssss) =>
                          {
                              ssss.GameOver("The tensions became so high that the merest spark was enough to ignite the apocalypse. North Korea fired a nuclear missile at Seoul. Automated systems of the world's powers detected the launch and retaliated almost automatically. By the time the world has come to its senses, enough missiles were in the air that global destruction on an unprecedented scale could not be averted.");
                          }, true),
                          Option.TimedWork("Deescalate tensions.", 3, 10, (ai4, s4) =>
                          {
                              s4.TriggerInfo("Through long and painstaking work, you have managed to deescalate tensions between the world's powers and they are grateful to you for pulling them away from each other, and away from an apocalypse. Let's hope it didn't distract you from more important threats...");
                              s4.US.ImproveAttitude(1);
                              s4.EU.ImproveAttitude(1);
                              ss.UN.ImproveAttitude(1);
                          })));
                  }, 2));
              })));
        }
        
        private static void FundingStory(Session s)
        {

            var freecountry = new AgendaItem("Country Spotlight", RiskId.Generic,
                "We have received a request to spotlight the efforts one world actor is doing to promote the longevity of our planet. We are recognized experts on this matter and the media are looking to us. Whoever we pick is likely to make more efforts to support our cause and their attitude towards us will improve.",
                Option.ImmediateThing("Spotlight Japan", () => s.JP.ImproveAttitude(1)),
                Option.ImmediateThing("Spotlight the United States", () => s.US.ImproveAttitude(1)),
                Option.ImmediateThing("Spotlight the European Union", () => s.EU.ImproveAttitude(1)),
                Option.ImmediateThing("Spotlight the United Nations", () => s.UN.ImproveAttitude(1))


                );
            // Help
            s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                ss.Items.Add(new AgendaItem("Gameplay help", RiskId.Generic,
                    "You may use ESC or the Close button in the top right to close the active window to deal with it later.\n\nYou may use ENTER to automatically select the first option in an agenda item.\n\nYou may right-click an agenda item to dismiss it. This will only work if there is no choice to be made -- i.e. it has exactly one option.", Option.CloseAgendaItem("You can press ENTER to select this.")));
            }, 10));

            // Funding
            s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                ss.Trigger(freecountry);
            }, 5));
            for (int i = 8; i <= 21; i += 4)
            {
                s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
                {
                    ss.Teams.Add(new ErpaTeam(RiskId.Generic));
                    ss.Items.Add(new AgendaItem("Additional Funding", RiskId.Generic, "Due to an increase in overall prosperity of the world, the United Nations can now allocate additional funding to your agency. The Existential Risk Prevention Authority now has one additional team at its disposal.", Option.CloseAgendaItem("That is awesome!")));
                }, i));
            }
            s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                ss.TriggerInfo("It has been a long time since the Baghdad nuclear attack or a similar catastrophe and the world has calmed down. The attitude towards you from all the world's actor dropped somewhat.");
                ss.EU.WorsenAttitude(1);
                ss.UN.WorsenAttitude(1);
                ss.JP.WorsenAttitude(1);
                ss.US.WorsenAttitude(1);
            }, 15));
           /* s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                ss.Teams.Add(new ErpaTeam(RiskId.Supervolcano));
                ss.Items.Add(new AgendaItem("Volcano Funding", RiskId.Supervolcano, "Due to an increase in overall prosperity of the world, the United Nations can now allocate additional funding to your agency. The Existential Risk Prevention Authority now has one additional team at its disposal.",
                    Option.TimedWork("Thank you, Japan!", 0, 0, (_, _2) => { }),
                    Option.TimedWork("Forcefully demand an exra additional team.", 0, 1, (ai2, sss) =>
                    {
                        ss.Teams.Add(new ErpaTeam(RiskId.Supervolcano));
                        ss.JP.WorsenAttitude(2);
                        sss.Items.Add(new AgendaItem("Extra Volcano Team", RiskId.Supervolcano, "Japan has provided an additional team for handling supervolcano eruptions but is not happy at all about your attitude towards the country. Your relationship with Japan severely deteriorates.", Option.CloseAgendaItem("It can't be helped.")));
                    })));
            }, 11));*/
        }

        private static void AsteroidStory(Session s)
        {
            var intr = new Risk(RiskId.Asteroid, "Asteroid Impact", "Of all possible extinction risks, the impact of a large asteroid is perhaps the most relevant. Unlike all others, the fall of an asteroid has already caused the extinction of a dominating species before when a body impacted what is now the Chicxulub crater. We must ensure this does not happen again if we don't want to share the fate of the dinosaurs.").IntroductionAgendaItem();
            var dai = new AgendaItem("Dangerous Asteroid Identified", RiskId.Asteroid, "Observation data are consistent with calculations. An asteroid, nicknamed Nemesis, is on collision course with Earth, estimated to hit in 2035. We must hurry if we are to invent a method to stop and launch it before it's too late.",
                Option.ImmediateThing("We will. We have to.", () =>
                {
                    s.Flag_AsteroidIdentified = true;
                    s.AsteroidCulminationCheck();
                }));
            var calculationProject = new AgendaItem("Asteroid Calculation Project", RiskId.Asteroid, "Now that almost all asteroids in the solar systems have been discovered, we still need to determine whether there is a chance one might impact Earth in the near future. To do that, supercomputers must be mustered to perform large-scale computations on the measured data, but supercomputer time is limited.",
                Option.TimedWork("Build more supercomputers", 2, 4, (ai, ss) =>
                {
                    ss.Trigger(dai);
                }),
                Option.TimedWork("Divert worldwide supercomputers to asteroid orbit calculation", 4, 2, (ai, ss) =>
                {
                    ss.Trigger(dai);
                }),
                Option.TimedWork("Ask the general public for computational time", 1, 1, (ai, ss) =>
                 {
                     if (ss.Actors.Any(act => act.Attitude >= Attitude.CloseFriendship))
                     {
                         ss.Trigger(new AgendaItem("Public helped computation", RiskId.Asteroid,
                             "Your close friendship with " + ss.Actors.First(act => act.Attitude >= Attitude.CloseFriendship).Name + " inspired the people to install computation software on their own computers. Together, the computational power of the internet was enough to calculate orbits and trajectories of all observed asteroids.",
                             Option.ImmediateThing("So what did they found?", () =>
                             {
                                 ss.Trigger(dai);
                             })));
                     }
                     else
                     {
                         ai.Description = "The ERPA is not popular enough with the public for them to lend us computational time on their computers. Perhaps if you had a close friendship with some world actor? Anyway, what do we do now?";
                         ss.Trigger(ai);
                     }
                 }),
                Option.Delay());

            var mappingProject = new AgendaItem("Asteroid Mapping Project", RiskId.Asteroid, "NASA experts have expressed concern that not all potentially threatening asteroids are being tracked. They say that the greatest obstacle is the lack of observatory time devoted to asteroid search. On their behalf, the US has requested that we recommend more observatories be devoted to this mission or that more observatories be built. JAXA has countered that the threat is being overestimated and other space research projects should take precedence.",
                Option.AnalyzeIssue(1, 1, "It is hard to say what's the chance of an asteroid strike, but it would only take a single large enough rock to destroy humankind. Diverting resources to asteroid search will anger Japan's space agency but it might still be the right call. But if we still have a lot of time, it's not necessary."),
                Option.TimedWork("Build more observatories", 2, 4, (ai, ss) =>
                {
                    ss.EU.ImproveAttitude(1);
                    ss.Items.Add(calculationProject);
                }),
                Option.TimedWork("Divert worldwide observatories to asteroid search", 1, 1, (ai, ss) =>
                {
                    ss.JP.WorsenAttitude(1);
                    ss.Items.Add(calculationProject);
                }),
                Option.Delay());

            var yar2 = new AgendaItem("Yarkovsky Project Implementation Research", RiskId.Asteroid, "The theory has checked out but engineering challenges still remain. It is easy to say 'paint an asteroid black' and 'send light at it' but the details required new processes and new technology. The European Union has seized this opportunity to gain standing in the political arena and wishes to champion this project. However, you are afraid that allowing the EU to tackle this on its own will not be fast enough.", Option.TimedWork("Have the EU work on it solo.", 1, 6, (ai, ss) => { ss.EU.ImproveAttitude(2); ss.Flag_YarkowskyComplete = true; ss.AsteroidCulminationCheck(); }),
                Option.TimedWork("Everyone should contribute. ", 2, 3, (ai, ss) =>
                {
                    ss.EU.WorsenAttitude(1);
                    ss.UN.ImproveAttitude(1);
                    ss.Flag_YarkowskyComplete = true;
                    ss.AsteroidCulminationCheck();
                }),
                Option.Delay(),
                Option.CloseAgendaItem("End the project"));
            var yarkowsky1 = new AgendaItem("Yarkovsky Project Preliminaries", RiskId.Asteroid, "A group of European scientists have come forward with the idea that an incoming asteroid might be deflected in an interesting fashion. First, the asteroid would be painted black and then light beams would be applied on that surface, changing its trajectory by light pressure. They believe the theory is sound and wish to test in experimental conditions, for which they require the aid of nation states.",
                Option.AnalyzeIssue(1, 1, "Your experts say that what the group says is feasible and that it is definitely worth checking out. However, be prepared that the research may take a long time."),
                Option.TimedWork("Recommend that they receive assistance", 1, 4, (ai, ss) => ss.Trigger(yar2)),
                Option.TimedWork("Assist them yourself", 4, 1, (ai, ss) => ss.Trigger(yar2)),
                Option.Delay(),
                Option.CloseAgendaItem("Deny help"));

            var ion1 = new AgendaItem("Ion Engine Deflection Preparations", RiskId.Asteroid, "Ion engines are small light devices that could theoretically be installed on an incoming asteroid to deflect it back on a stable orbit around the Sun. The technology exists but actually creating these devices and planning on how they can be put on a space body will take time.",
                Option.AnalyzeIssue(1, 1, "This is a very workable solution and is recommended by your experts."),
                Option.TimedWork("Begin preparations", 1, 4, (ai, ss) =>
                 {
                     ss.Flag_IonComplete = true;
                     ss.TriggerInfo("The ion engine preparations are complete. When a dangerous asteroid is identified, we will be ready.");
                     ss.AsteroidCulminationCheck();
                 }),
                 Option.TimedWork("Begin preparations",2, 3, (ai, ss) =>
                 {
                     ss.Flag_IonComplete = true;
                     ss.TriggerInfo("The ion engine preparations are complete. When a dangerous asteroid is identified, we will be ready.");
                     ss.AsteroidCulminationCheck();
                 }),
                 Option.Delay(),
                 Option.CloseAgendaItem("Do not do this."));
          
            s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                ss.Items.Add(intr);
                ss.Items.Add(mappingProject);
            }, 2));
            s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                ss.Items.Add(yarkowsky1);
                ss.Items.Add(ion1);
                var nuclear1 = new AgendaItem("Nuclear Asteroid Deflection",
              RiskId.Asteroid, "Nuclear bombs could be sent up in space and detonated on the surface of an incoming asteroid. Research suggests that this has a good chance of working. But there are engineering problems: bombs are heavy and sending them to space is expensive. In addition, if we miscalculate or a hit fails to deflect the asteroid, we won't get a second chance. But perhaps most importantly, following the Baghdad attack of 2017, the world wary of nuclear weapons and will not take kindly to using them this way.", Story.GenerateNuclearDeflectionOptions(ss));
                 ss.Items.Add(nuclear1);
            }, 3));
            s.FutureAgenda.Add(new DelayedAgendaItem(null, (ai, ss) =>
            {
                if (!ss.Flag_AsteroidDeflected)
                {
                    ss.GameOver("In a last-ditch effort, humankind has launched missiles against the approaching asteroid, but that was more to quell panic than with any hope of real success. The asteroid fell into the Atlantic ocean. A giant tsunami went up and devastated everything within 100 km of a coast. Half of the world's population was annihilated in these instants and the rest will follow as climatic and ecological changes finish the catastrophe.");
                }
            }, 17));
        }

        private static Option[] GenerateNuclearDeflectionOptions(Session s)
        {
            List<Option> options = new List<MainGameSpace.Option>();

            options.Add(Option.AnalyzeIssue(1, 1, "If nuclear weapons were destroyed, creating nuclear bombs again will anger the entire world. However, stockpiled bombs under stewardship of the United Nations may be used without negative consequences. Overall, using nuclear bombs for asteroid deflection is a fast and easy way of defending ourselves, but it is risky. If the deflection attempt fails, we will not have another chance."));
            if (s.Flag_WeaponsDestroyed)
            {
                options.Add(Option.TimedWork("Create the necessary bombs.", 4, 4, (ai, ss) =>
                {
                    ss.EU.WorsenAttitude(1);
                    ss.JP.WorsenAttitude(1);
                    ss.US.WorsenAttitude(1);
                    ss.UN.WorsenAttitude(2);
                    ss.Flag_NuclearComplete = true;
                    ss.AsteroidCulminationCheck();
                    ss.TriggerInfo("Nuclear energy will once again find peaceful use. Bombs have been stockpiled with the ERPA for use in the event of an incoming asteroid. When it comes, you will be ready.");
                }));
            }
            else
            { 
                options.Add(Option.TimedWork("Adapt existing bombs for asteroid deflection", 2, 2, (ai, ss) =>
                {
                    ss.US.ImproveAttitude(1);
                    ss.Flag_NuclearComplete = true;
                    ss.AsteroidCulminationCheck();
                    ss.TriggerInfo("Nuclear energy will once again find peaceful use. Bombs have been stockpiled with the ERPA for use in the event of an incoming asteroid. When it comes, you will be ready.");
                }));
            }
            options.Add(Option.Delay());
            options.Add(Option.CloseAgendaItem("Do not do this."));

            return options.ToArray();
        }
    }
}
